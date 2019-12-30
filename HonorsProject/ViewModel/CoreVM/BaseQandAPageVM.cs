﻿using HonorsProject.Model.Core;
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
    public abstract class BaseQandAPageVM : BaseViewModel, ISaveVMFormCmd,IDeleteCmd,IUploadImageCmd,IToggleMarkQCmd,ICancelmd
    {
        #region Properties

        public abstract ISystemUser User {get;set;}
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
                if (SelectedQuestion != null)
                    Answers = new ObservableCollection<Answer>(UnitOfWork.AnswerRepository.GetFromSession(SelectedQuestion).ToList());
                else
                    Answers = new ObservableCollection<Answer>();
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
            set
            {
                if (value == null)
                    value = new Answer();
                //if selected.id == 0 create else update
                FormContextAnswer = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedQuestion));
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
        public bool IsConfirmed { get; set; }

        #endregion Properties
        #region Commands
        public SaveCmd SaveFormCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set ; }
        public UploadImageCmd UploadImageCmd { get; set; }
        public ToggleMarkQCmd ToggleMarkQCmd { get; set ; }
        public CancelCmd CancelCmd { get ; set; }
        #endregion Commands
        public BaseQandAPageVM(string dbcontextName) : base(dbcontextName)
        {
            //Commands
            SaveFormCmd = new SaveCmd(this);
            DeleteCmd = new DeleteCmd(this);
            UploadImageCmd = new UploadImageCmd(this);
            ToggleMarkQCmd = new ToggleMarkQCmd(this);
            CancelCmd = new CancelCmd(this);

        }

        public abstract bool Save();

        public abstract bool Delete(BaseEntity objToDelete);

        public abstract bool UploadImage(Image imageToUpload);

        public abstract bool MarkQuestion(Question questionToMark);

        public abstract bool Cancel();
        
    }
}
