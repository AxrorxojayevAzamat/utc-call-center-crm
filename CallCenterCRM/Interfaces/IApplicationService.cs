﻿using CallCenterCRM.Models;

namespace CallCenterCRM.Interfaces
{
    public interface IApplicationService
    {
        public string GetAppNumber(Application item);
        public StatusParam GetAppStatusParams(ApplicationStatus status);
        public StatusParam GetAnswerStatusParams(AnswerStatus status);
        public StatusParam GetStatusForOperator(Application application);
        public int AppCount(int recipentId, ApplicationStatus status);
        public int AnswerCount(int userId, AnswerStatus status);

        public bool ShowReason(Application app, string role);

        public bool IsGot(Roles role, ApplicationStatus status);
        public bool IsGotAnswer(Roles role, AnswerStatus status);
    }
}
