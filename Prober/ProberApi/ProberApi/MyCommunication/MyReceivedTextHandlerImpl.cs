using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CommonApi.MyCommunication;
using CommonApi.MyEnum;
using CommonApi.MyUtility;

using NLog;

using ProberApi.MyBoard;
using ProberApi.MyConstant;
using ProberApi.MyQuery;
using ProberApi.MyRequest;

namespace ProberApi.MyCommunication {
    public sealed class MyReceivedTextHandlerImpl : MyReceivedTextHandler {
        public MyReceivedTextHandlerImpl(ConcurrentDictionary<string, object> sharedObjects) {
            sharedObjects.TryGetValue(SharedObjectKey.QUERY_LIST, out object tempObj);
            this.queryList = tempObj as List<AbstractQuery>;
            sharedObjects.TryGetValue(SharedObjectKey.REQUEST_STATUS_BOARD, out tempObj);
            this.requestStatusBoard = tempObj as RequestStatusBoard;
            sharedObjects.TryGetValue(SharedObjectKey.TEMPLATE_REQUEST_LIST, out tempObj);
            this.templateRequestList = tempObj as List<AbstractRequest>;
        }

        public void PrepareHandling() {
            cts = new CancellationTokenSource();

            _ = Task.Run(async () => {
                while (true) {
                    if (cts.IsCancellationRequested) {
                        //[gyh,20240618]: following 6 lines code is newly added!
                        if (requestQueue.Count > 0) {
                            LOGGER.Warn($"There are still {requestQueue.Count} remote requests in waiting queue, they will be abandoned!");
                            while (!requestQueue.IsEmpty) {
                                requestQueue.TryDequeue(out _);
                            }
                        }
                        return;
                    }

                    if (!requestQueue.TryDequeue(out var request)) {
                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                        continue;
                    }

                    _ = AsyncHandleRequest(request);
                }
            });
        }

        public string Handle(string receivedText) {
            string receivedTextUppercase = receivedText.ToUpperInvariant().Trim();
            if (receivedTextUppercase.StartsWith(CommunicationConstant.QUERY_IF_COMPLETED)) {
                string requestSerailNum = receivedText.Substring(CommunicationConstant.QUERY_IF_COMPLETED.Length);
                return QueryIfCompleted(requestSerailNum);
            } else if (receivedTextUppercase.StartsWith(CommunicationConstant.REQUEST)) {
                string fullRequest = receivedText.Substring(CommunicationConstant.REQUEST.Length);
                return HandleFullRequestText(fullRequest.Trim());
            } else if (receivedTextUppercase.StartsWith(CommunicationConstant.WAIT_FOR_COMPLETED)) {
                string requestSerailNum = receivedText.Substring(CommunicationConstant.WAIT_FOR_COMPLETED.Length);
                return WaitForCompleted(requestSerailNum);
            } else if (receivedTextUppercase.StartsWith(CommunicationConstant.QUERY)) {
                string fullQuery = receivedText.Substring(CommunicationConstant.QUERY.Length).Trim();
                return HandleQuery(fullQuery.Trim());                
            } else {
                LOGGER.Error($"request text(={receivedText}) is invalid!");
                return $"{(int)EnumResponseId.INVALID_REQUEST_TYPE},";
            }
        }

        public void AbortHandling() {
            cts.Cancel();
            /*
            if (requestQueue.Count == 0) {
                return;
            }
            LOGGER.Warn($"There are still {requestQueue.Count} remote requests in waiting queue, they will be abandoned!");
            while (!requestQueue.IsEmpty) {
                requestQueue.TryDequeue(out var temp);
            }
            */
        }

        public string HandleQuery(string fullQueryText) {
            if (string.IsNullOrEmpty(fullQueryText)) {
                return $"{(int)EnumResponseId.INVALID_QUERY_TYPE},";
            }
            string[] parts = fullQueryText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 2) {
                return $"{(int)EnumResponseId.INVALID_QUERY_TYPE},";
            }
            string queryType = parts[0];
            string queryParameter = string.Empty;
            if (parts.Length > 1) {
                queryParameter = parts[1];
            }
            return Query(queryType, queryParameter);
        }

        private string Query(string queryTypeId, string parameter) {
            string queryTypeIdUppercase = queryTypeId.ToUpperInvariant().Trim();
            var choosenList = queryList.Where(q => q.QueryType.Equals(queryTypeIdUppercase)).ToList();
            if (choosenList.Count == 0) {
                return $"{(int)EnumResponseId.INVALID_QUERY_TYPE},";
            }
            var query = choosenList.First();
            var queryResult = query.Query(parameter);
            if (queryResult.isOk) {
                return $"{(int)EnumResponseId.PASS},{queryResult.result}";
            } else {
                return $"{(int)EnumResponseId.FAIL},{queryResult.result}";
            }
        }

        //[gyh]: to be added 超时机制！
        private string WaitForCompleted(string requestSerailNum) {
            string requestSerailNumUppercase = requestSerailNum.ToUpperInvariant().Trim();
            if (!requestStatusBoard.ContainsKey(requestSerailNumUppercase)) {
                return $"{(int)EnumResponseId.INVALID_REQUEST_SERIAL_NUMBER},";
            }

            while (true) {
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
                if (requestStatusBoard.IsCompleted(requestSerailNumUppercase)) {
                    var requestStatus = requestStatusBoard[requestSerailNum];
                    int result = (int)(requestStatus.requestStatus);
                    requestStatusBoard.RemoveItem(requestSerailNumUppercase);
                    return $"{(int)EnumResponseId.PASS},{result}";
                }
            }
        }

        private string QueryIfCompleted(string requestSerailNum) {
            string requestSerailNumUppercase = requestSerailNum.ToUpperInvariant().Trim();
            if (!requestStatusBoard.ContainsKey(requestSerailNumUppercase)) {
                return $"{(int)EnumResponseId.INVALID_REQUEST_SERIAL_NUMBER},";
            }

            return $"{(int)EnumResponseId.PASS},{(int)(requestStatusBoard[requestSerailNumUppercase].requestStatus)}";
        }

        private string HandleFullRequestText(string fullRequestText) {
            var tryResult = TryGenerateRequest(fullRequestText);
            if (!tryResult.isOk) {
                return tryResult.errorReponse;
            }

            AbstractRequest request = tryResult.request;
            requestStatusBoard.AddNew(request.SerialNumber, request.RequestType);
            requestQueue.Enqueue(request);
            return $"{(int)(EnumResponseId.PASS)},{request.SerialNumber}";
        }

        private Task AsyncHandleRequest(AbstractRequest request) {
            return Task.Run(() => {
                requestStatusBoard.UpdateBeginRunning(request.SerialNumber);

                EnumRequestStatus status = EnumRequestStatus.RUNNING;
                (int responseId, string runResult, object attachedData) runResult = ((int)EnumRequestStatus.COMPLETED_PASS, string.Empty, null);
                try {
                    runResult = request.Run();
                    if (runResult.responseId == (int)EnumResponseId.PASS) {
                        status = EnumRequestStatus.COMPLETED_PASS;
                    } else {
                        status = EnumRequestStatus.COMPLETED_FAIL;
                    }
                } catch (Exception ex) {
                    LOGGER.Error(MyLogUtility.GenerateExceptionLog(ex));
                    status = EnumRequestStatus.COMPLETED_EXCEPTION;
                    runResult = ((int)EnumRequestStatus.COMPLETED_EXCEPTION, ex.Message, runResult.attachedData);
                } finally {
                    requestStatusBoard.UpdateCompleted(request.SerialNumber, status, runResult.runResult);
                }
            });
        }

        private (bool isOk, string errorReponse, AbstractRequest request) TryGenerateRequest(string fullRequestText) {
            string[] parts = fullRequestText.Split(' ');
            string requestType = parts[0].Trim().ToUpperInvariant();

            var list = templateRequestList.Where(x => x.RequestType.ToUpperInvariant().Equals(requestType)).ToList();
            if (list.Count == 0) {
                return (false, $"{(int)(EnumResponseId.INVALID_REQUEST_TYPE)},", null);
            }

            AbstractRequest templateRequest = list.First();
            AbstractRequest request = templateRequest.DeepCopyDefaultInstance();
            if (parts.Length == 1) {
                return (true, string.Empty, request);
            }

            string parameters = parts[1].Trim();
            var tryResult = request.TryUpdateParameters(parameters);
            if (!tryResult.isOk) {
                return (false, $"{(int)(EnumResponseId.INVALID_REQUEST_PARAMETERS)},{tryResult.errorMessage}", null);
            }

            return (true, string.Empty, request);
        }

        private readonly List<AbstractQuery> queryList;
        private readonly RequestStatusBoard requestStatusBoard;
        private readonly List<AbstractRequest> templateRequestList;
        private readonly ConcurrentQueue<AbstractRequest> requestQueue = new ConcurrentQueue<AbstractRequest>();
        private CancellationTokenSource cts;
        private static readonly Logger LOGGER = LogManager.GetLogger(MyLogUtility.DEFAULT_FILE_LOGGER);
    }
}
