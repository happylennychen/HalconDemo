using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

using CommonApi.MyCommunication;
using CommonApi.MyI18N;
using CommonApi.MyUtility;

namespace ProberApi.MyCommunication {
    public sealed class MyServerSideImpl : MyServerSide {
        public MyServerSideImpl(EnumLanguage language, ConcurrentDictionary<string, object> sharedObjects, Action<bool> actSetGuiEnableStatus) : base(sharedObjects, actSetGuiEnableStatus) {
            clientConnected = false;
            rc = new ResourceCulture($"{Assembly.GetExecutingAssembly().GetName().Name}.GlobalStrings", Assembly.GetExecutingAssembly());
            rc.Language = language;
        }

        public override void Listen(string ip, int port, int maxConnectionNum) {
            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.ExclusiveAddressUse = true;
            serverSocket.Bind(endPoint);
            serverSocket.Listen(maxConnectionNum);
            this.ip = ip;
            this.port = port;
            AcceptConnection();
        }

        private void AcceptConnection() {
            if (!serverSocket.Connected) {
                reportMessage?.Invoke($"{rc.GetString("txtStartListening")}({ip}:{port})......");
                serverSocket.BeginAccept(AcceptedCallback, null);
            }
        }

        private void AcceptedCallback(IAsyncResult ar) {
            if (serverSocket == null) {
                return;
            }

            try {
                workerSocket = serverSocket.EndAccept(ar);
                actSetGuiEnableStatus.Invoke(false);
                clientConnected = true;

                workerSocket.ReceiveBufferSize = CommunicationConstant.BUFF_SIZE;
                workerSocket.SendBufferSize = CommunicationConstant.BUFF_SIZE;
                IPEndPoint localEndPoint = workerSocket.LocalEndPoint as IPEndPoint;
                IPEndPoint remoteEndPoint = workerSocket.RemoteEndPoint as IPEndPoint;
                //reportMessage?.Invoke($"A new TCP connection(local={localEndPoint.Address}:{localEndPoint.Port}, remote={remoteEndPoint.Address}:{remoteEndPoint.Port}) is established.");
                reportMessage?.Invoke($"A client({remoteEndPoint.Address}:{remoteEndPoint.Port}) has connected.");

                CommunicatingWithClient(workerSocket);
            } catch (ObjectDisposedException ode) {
                LOGGER.Error(ode.Message);
                ReportMessage(ode.Message);
            } catch (Exception ex) {
                LOGGER.Error(ex.Message);
                ReportMessage(ex.Message);
            }
        }

        private void CommunicatingWithClient(Socket workerSocket) {
            try {
                receivedTextHandler.PrepareHandling();

                while (clientConnected) {
                    Thread.Sleep(TimeSpan.FromMilliseconds(50));

                    byte[] bytesRequest = Receive(workerSocket);
                    if (bytesRequest.Length == 0) {
                        continue;
                    } else {
                        LOGGER.Debug($"Byte length of request={bytesRequest.Length}");
                    }
                    string content = Encoding.UTF8.GetString(bytesRequest, 0, bytesRequest.Length);
                    string request = content.Trim();
                    if (string.IsNullOrEmpty(request)) {
                        continue;
                    }
                    reportMessage?.Invoke($"[Request]:{request}");

                    if (request.Equals(CommunicationConstant.SAY_GOODBYE)) {
                        DiconnectClient();
                        reportMessage?.Invoke($"Client has closed current connection.");
                        actSetGuiEnableStatus(true);
                        return;
                    }

                    string response = receivedTextHandler.Handle(request);
                    if (string.IsNullOrEmpty(response)) {
                        //[gyh]应如何处理????
                        return;
                    }

                    byte[] bytesResponse = Encoding.UTF8.GetBytes(response);
                    Send(workerSocket, bytesResponse);
                    reportMessage?.Invoke($"[Response]:{response}");
                }
            } catch (SocketException se) {
                LOGGER.Error(se.Message);
                ReportMessage(se.Message);
            } catch (Exception ex) {
                LOGGER.Error(ex.Message);
                ReportMessage(ex.Message);
            } finally {
                DiconnectClient();
                ReportMessage("Connection has been closed!");
                actSetGuiEnableStatus(true);

                clientConnected = false;
                AcceptConnection();
            }
        }

        public override bool IsListening() {
            if (serverSocket == null) {
                return false;
            }

            return serverSocket.IsBound;
        }

        public override void Close() {
            if (!serverSocket.IsBound) {
                return;
            }

            string message = "Forced abort listening.";
            reportMessage?.Invoke(message);
            LOGGER.Info(message);
            try {
                serverSocket.Close();
                serverSocket.Dispose();
                serverSocket = null;
            } catch (Exception ex) {
                LOGGER.Error("serverSocket.Close() " + MyLogUtility.GenerateExceptionLog(ex));
            }
        }

        public override void DiconnectClient() {
            if (workerSocket == null) {
                return;
            }

            try {
                workerSocket.Shutdown(SocketShutdown.Both);
                workerSocket.Close();
                workerSocket.Dispose();
                workerSocket = null;
            } catch (Exception ex) {
                LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
            }
        }

        public override void AbortHandling() {
            receivedTextHandler.AbortHandling();
        }

        public override MyServerSide DeepCopy() {
            MyServerSideImpl result = new MyServerSideImpl(rc.Language, this.sharedObjects, this.actSetGuiEnableStatus);
            this.AttachReportMessage(this.reportMessage);
            return result;
        }

        private byte[] Receive(Socket workerSocket) {
            byte[] bytesRequestLength = new byte[CommunicationConstant.BYTE_NUM_OF_LENGTH];
            int actualLengthPerRecved = 0;
            lock (theMutex) {
                actualLengthPerRecved = workerSocket.Receive(bytesRequestLength, bytesRequestLength.Length, SocketFlags.None);
            }
            if (HasRemoteEndpointClosedConnection(actualLengthPerRecved)) {
                throw new MyRemoteDisconnectedException();
            }
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytesRequestLength);
            }
            int expectedRequestLength = BitConverter.ToInt32(bytesRequestLength, 0);
            if (expectedRequestLength == 0) {
                return new byte[0];
            }

            byte[] bytesRequest = new byte[expectedRequestLength];
            byte[] buffer = new byte[CommunicationConstant.BUFF_SIZE];
            int actualReceivedTotalLength = 0;
            int expectedLengthPerRecv = 0;
            int remainLength = bytesRequest.Length;
            while (remainLength > 0) {
                if (remainLength > CommunicationConstant.BUFF_SIZE) {
                    expectedLengthPerRecv = CommunicationConstant.BUFF_SIZE;
                } else {
                    expectedLengthPerRecv = remainLength;
                }

                Array.Clear(buffer, 0, buffer.Length);
                lock (theMutex) {
                    actualLengthPerRecved = workerSocket.Receive(buffer, expectedLengthPerRecv, SocketFlags.None);
                }
                if (HasRemoteEndpointClosedConnection(actualLengthPerRecved)) {
                    throw new MyRemoteDisconnectedException();
                }
                Array.Copy(buffer, 0, bytesRequest, actualReceivedTotalLength, actualLengthPerRecved);

                actualReceivedTotalLength += actualLengthPerRecved;
                remainLength -= actualLengthPerRecved;
            }

            if (actualReceivedTotalLength != expectedRequestLength) {
                LOGGER.Error($"request length(bytes)={expectedRequestLength}, actual sent length={actualReceivedTotalLength}");
            }

            return bytesRequest;
        }

        private int Send(Socket workerSocket, byte[] bytesResponse) {
            byte[] bytesResponseLength = BitConverter.GetBytes(bytesResponse.Length);
            if (BitConverter.IsLittleEndian) {
                Array.Reverse(bytesResponseLength);
            }
            byte[] bytesTotal = new byte[bytesResponseLength.Length + bytesResponse.Length];
            Array.Copy(bytesResponseLength, 0, bytesTotal, 0, bytesResponseLength.Length);
            Array.Copy(bytesResponse, 0, bytesTotal, bytesResponseLength.Length, bytesResponse.Length);

            byte[] buffer = new byte[CommunicationConstant.BUFF_SIZE];
            int remainLength = bytesTotal.Length;
            int expectedLengthPerSend = 0;
            int actualTotalLength = 0;
            while (remainLength > 0) {
                if (remainLength > CommunicationConstant.BUFF_SIZE) {
                    expectedLengthPerSend = CommunicationConstant.BUFF_SIZE;
                } else {
                    expectedLengthPerSend = remainLength;
                }

                Array.Clear(buffer, 0, buffer.Length);
                Array.Copy(bytesTotal, bytesTotal.Length - remainLength, buffer, 0, expectedLengthPerSend);
                int actualLengthPerSent = 0;
                lock (theMutex) {
                    actualLengthPerSent = workerSocket.Send(buffer, expectedLengthPerSend, SocketFlags.None);
                }
                actualTotalLength += actualLengthPerSent;
                remainLength -= actualLengthPerSent;
            }

            if (actualTotalLength != bytesTotal.Length) {
                LOGGER.Error($"expected send length(bytes)={bytesTotal.Length}, actual sent length={actualTotalLength}");
            }
            return actualTotalLength;
        }

        private static bool HasRemoteEndpointClosedConnection(int recvByteNum) {
            return recvByteNum == 0;
        }

        private bool clientConnected;
        private string ip;
        private int port;
        private Socket workerSocket;
        private readonly object theMutex = new object();
        private readonly ResourceCulture rc;
    }
}
