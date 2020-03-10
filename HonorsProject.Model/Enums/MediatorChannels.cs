using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.Model.Enums
{
    public enum MediatorChannels
    {
        DeleteSessionConfirmation,
        LoginAsUserX,
        StudentsPageGeneratePasswordCheck,
        StudentsPageNewPasswordDisplay,
        LoadGroupsSubgridForStudents,
        LoadQuestionsSubgridForStudents,
        DeleteStudentConfirmation,
        DeleteGroupConfirmation,
        LoadActiveSessionsSubgrid,
        LoadFutureSessionsSubgrid,
        LoadPreviousSessionsSubgrid,
        LoadStudentsSubgrid,
        GoToThisSession,
        DeleteQuestionConfirmation,
        DeleteAnswerConfirmation,
        ChangePasswordBox,
        ClearPropPassInput,
        GoToThisGroup,
        GoToThisStudent,
        GoToThisQuestion,
        StudnetCSVImport,
        LecturerPageGeneratePasswordCheck,
        LecturerPageNewPasswordDisplay,
        GoToAnalyseEntity,
        PoolingUpdate,
        DeleteLecturerConfirmation
    }
}