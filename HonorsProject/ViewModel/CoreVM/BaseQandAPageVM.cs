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

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseQandAPageVM : BaseViewModel, ISaveVMFormCmd,IDeleteCmd,IUploadImageCmd,IToggleMarkQCmd
    {
        #region Properties

        public abstract ISystemUser User {get;set;}
        private FormContext _formContext;

        public FormContext FormContext
        {
            get { return _formContext; }
            set { _formContext = value;
                OnPropertyChanged(nameof(FormContext));
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
            set { _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
            }
        }
        private string _quesitonSearchTxt;

        public string QuestionSearchTxt
        {
            get { return _quesitonSearchTxt; }
            set { _quesitonSearchTxt = value;
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
            set { _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
            }
        }
        private string _answerSearchTxt;

        public string AnswerSearchTxt
        {
            get { return _answerSearchTxt; }
            set { _answerSearchTxt = value;
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
        #endregion Properties
        #region Commands
        public SaveCmd SaveFormCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set ; }
        public UploadImageCmd UploadImageCmd { get; set; }
        public ToggleMarkQCmd ToggleMarkQCmd { get; set ; }
        #endregion Commands
        public BaseQandAPageVM(string dbcontextName) : base(dbcontextName)
        {
        }

        public abstract bool Save();

        public abstract bool Delete(BaseEntity objToDelete);

        public abstract bool UploadImage(Image imageToUpload);

        public abstract bool MarkQuestion(Question questionToMark);
    }
}
