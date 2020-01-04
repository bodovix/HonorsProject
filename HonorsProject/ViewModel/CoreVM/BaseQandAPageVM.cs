using HonorsProject.Model.Core;
using HonorsProject.Model.Entities;
using HonorsProject.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using HonorsProject.ViewModel.Commands.IComands;
using HonorsProject.ViewModel.Commands;
using HonorsProject.Model.DTO;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseQandAPageVM : BaseViewModel, ISaveVMFormCmd,IDeleteCmd,IUploadImageCmd,IToggleMarkQCmd,ICancelmd,IEnterNewModeCmd
    {
        #region Properties

        public abstract ISystemUser User { get; set; }

        private FormContext _formContextQuestion;

        public FormContext FormContextQuestion
        {
            get { return _formContextQuestion; }
            set { _formContextQuestion = value;
                OnPropertyChanged(nameof(FormContextQuestion));
            }
        }
        private FormContext _formContextAnswer;

        public FormContext FormContextAnswer
        {
            get { return _formContextAnswer; }
            set
            {
                _formContextAnswer = value;
                OnPropertyChanged(nameof(FormContextAnswer));
            }
        }
        private Session _selectedSession;

        public Session SelectedSession
        {
            get { return _selectedSession; }
            set { _selectedSession = value;
                OnPropertyChanged(nameof(SelectedSession));
            }
        }

        private ObservableCollection<Question> _questions;

        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set { _questions = value;
                OnPropertyChanged(nameof(Questions));
            }
        }
        private Question _selectedQuestion;

        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set {
                if (value == null)
                    value = new Question();
                //if selected.id == 0 create else update
                FormContextQuestion = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedQuestion = value;
                UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                OnPropertyChanged(nameof(SelectedQuestion));
                VisConverterDTO.Question = value;
                OnPropertyChanged(nameof(VisConverterDTO));
            }
        }

        private string _quesitonSearchTxt;

        public string QuestionSearchTxt
        {
            get { return _quesitonSearchTxt; }
            set { _quesitonSearchTxt = value;
                UpdateQuestionsList( SelectedSession, QuestionSearchTxt);
                OnPropertyChanged(nameof(QuestionSearchTxt));
            }
        }

        private Image _questionImage;

        public Image QuestionImage
        {
            get { return _questionImage; }
            set
            {
                _questionImage = value;
                OnPropertyChanged(nameof(QuestionImage));
            }
        }

        private ObservableCollection<Answer> _answers;

        public ObservableCollection<Answer> Answers
        {
            get { return _answers; }
            set { _answers = value;
                OnPropertyChanged(nameof(Answers));
            }
        }
        private Answer _selectedAnswer;

        public Answer SelectedAnswer
        {
            get { return _selectedAnswer; }
            set
            {
                if (value == null)
                    value = new Answer();
                //if selected.id == 0 create else update
                FormContextAnswer = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }
        private string _answerSearchTxt;

        public string AnswerSearchTxt
        {
            get { return _answerSearchTxt; }
            set { _answerSearchTxt = value;
                UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                OnPropertyChanged(AnswerSearchTxt);
            }
        }

        private Image _answerImage;

        public Image AnswerImage
        {
            get { return _answerImage; }
            set { _answerImage = value;
                OnPropertyChanged(nameof(AnswerImage));
            }
        }

        private QandAMode _qandAMode;

        public QandAMode QandAMode
        {
            get { return _qandAMode; }
            set { _qandAMode = value;
                OnPropertyChanged(nameof(QandAMode));
            }
        }

        private QuestionVisConverterDTO _visConverterDTO;

        public QuestionVisConverterDTO VisConverterDTO
        {
            get { return _visConverterDTO; }
            set
            {
                _visConverterDTO = value;
                OnPropertyChanged(nameof(VisConverterDTO));
            }
        }

        public bool IsConfirmed { get; set; }
        #endregion Properties
        #region Commands
        public SaveCmd SaveFormCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set ; }
        public UploadImageCmd UploadImageCmd { get; set; }
        public ToggleMarkQCmd ToggleMarkQCmd { get; set ; }
        public CancelCmd CancelCmd { get ; set; }
        public NewModeCmd NewModeCmd { get; set; }
        #endregion Commands
        public BaseQandAPageVM(string dbcontextName) : base(dbcontextName)
        {
            //Commands
            SaveFormCmd = new SaveCmd(this);
            DeleteCmd = new DeleteCmd(this);
            UploadImageCmd = new UploadImageCmd(this);
            ToggleMarkQCmd = new ToggleMarkQCmd(this);
            CancelCmd = new CancelCmd(this);
            NewModeCmd = new NewModeCmd(this);

            AnswerSearchTxt = "";
            QuestionSearchTxt = "";
        }
        protected abstract bool UpdateQuestionsList(Session SelectedSession, string QuestionSearchTxt);
        protected abstract bool UpdateAnswersList(Question SelectedQuestion, string AnswerSearchTxt);

        public abstract bool Save();

        public abstract bool Delete(BaseEntity objToDelete);

        public abstract bool UploadImage(Image imageToUpload);

        public bool MarkQuestion(Question questionToMark)
        {
            bool result = false;
            if(questionToMark != null)
            {
                if(questionToMark.Id > 0)
                {
                    //toggle is Resolved for question
                    questionToMark.IsResolved = !questionToMark.IsResolved;
                    result = (UnitOfWork.Complete() > 0)? true : false;
                    if (result == false)
                        FeedbackMessage = "Unable to mark question as resolved.";
                }
                else
                    FeedbackMessage = "No question selected.";
            }
            else
                FeedbackMessage = "No question selected.";
            return result;
        }
        public abstract bool Cancel();

        public abstract void EnterNewMode();
        
    }
}
