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
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using HonorsProject.Model.HelperClasses;

namespace HonorsProject.ViewModel.CoreVM
{
    public abstract class BaseQandAPageVM : BaseViewModel, ISaveVMFormCmd, IDeleteCmd, IUploadImageCmd, IToggleMarkQCmd, IToggleMarkACmd, ICancelmd, IEnterNewModeCmd
    {
        #region Properties

        public abstract ISystemUser User { get; set; }
        private string _headerMessage;

        private string _questionImageLabel;

        public string QuestionImageLabel
        {
            get { return _questionImageLabel; }
            set
            {
                _questionImageLabel = value;
                OnPropertyChanged(nameof(QuestionImageLabel));
            }
        }

        private string _answerImageLabel;

        public string AnswerImageLabel
        {
            get { return _answerImageLabel; }
            set
            {
                _answerImageLabel = value;
                OnPropertyChanged(nameof(AnswerImageLabel));
            }
        }

        public string HeaderMessage
        {
            get { return _headerMessage; }
            set
            {
                _headerMessage = value;
                OnPropertyChanged(nameof(HeaderMessage));
            }
        }

        private FormContext _formContextQuestion;

        public FormContext FormContextQuestion
        {
            get { return _formContextQuestion; }
            set
            {
                _formContextQuestion = value;
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
            set
            {
                _selectedSession = value;
                OnPropertyChanged(nameof(SelectedSession));
            }
        }

        private ObservableCollection<Question> _questions;

        public ObservableCollection<Question> Questions
        {
            get { return _questions; }
            set
            {
                _questions = value;
                OnPropertyChanged(nameof(Questions));
            }
        }

        private Question _selectedQuestion;

        public Question SelectedQuestion
        {
            get { return _selectedQuestion; }
            set
            {
                if (value == null)
                    value = new Question();
                ClearFeedback();

                //if selected.id == 0 create else update
                FormContextQuestion = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedQuestion = value;
                UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                OnPropertyChanged(nameof(SelectedQuestion));
                QVisConDTO.Question = value;
                OnPropertyChanged(nameof(QVisConDTO));
                SetHeaderMessage();
                //if selected question has image. download it
                AsyncRunner.Run(AwaitQuestionImage());
            }
        }

        private async Task AwaitQuestionImage()
        {
            QuestionImageLabel = "Loading Image...";
            QuestionImage = await RefreshImage(nameof(QuestionImage), SelectedQuestion.ImageLocation);
            if (QuestionImage == null)
                QuestionImageLabel = "No Image.";
            else
                QuestionImageLabel = SelectedQuestion.ImageLocation;
        }

        private async Task AwaitAnswerImage()
        {
            AnswerImageLabel = "Loading Image...";
            AnswerImage = await RefreshImage(nameof(AnswerImage), SelectedAnswer.ImageLocation);
            if (AnswerImage == null)
                AnswerImageLabel = "No Image.";
            else
                AnswerImageLabel = SelectedAnswer.ImageLocation;
        }

        private async Task<ImageSource> RefreshImage(string nameOfImageProperty, string imageLocation)
        {
            ImageSource imageSource;
            if (!String.IsNullOrEmpty(imageLocation))
            {
                byte[] imageByteArray = await ImageHandler.ReadByteArrayFromSFTP(imageLocation);
                if (imageByteArray != null)
                    imageSource = ImageHandler.ByteToImage(imageByteArray);
                else
                    imageSource = null;
            }
            else
                imageSource = null;
            OnPropertyChanged(nameOfImageProperty);

            return imageSource;
        }

        public abstract string QuestionSearchTxt { get; set; }

        private ImageSource _questionImage;

        public ImageSource QuestionImage
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
            set
            {
                _answers = value;
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
                ClearFeedback();

                //if selected.id == 0 create else update
                FormContextAnswer = (value.Id == 0) ? FormContext.Create : FormContext.Update;
                _selectedAnswer = value;
                OnPropertyChanged(nameof(SelectedAnswer));
                AVisConDTO.Answer = value;
                OnPropertyChanged(nameof(AVisConDTO));
                SetHeaderMessage();
                //if selected answer has image. download it
                AsyncRunner.Run(AwaitAnswerImage());
            }
        }

        private string _answerSearchTxt;

        public string AnswerSearchTxt
        {
            get { return _answerSearchTxt; }
            set
            {
                _answerSearchTxt = value;
                UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                OnPropertyChanged(AnswerSearchTxt);
            }
        }

        private ImageSource _answerImage;

        public ImageSource AnswerImage
        {
            get { return _answerImage; }
            set
            {
                _answerImage = value;
                OnPropertyChanged(nameof(AnswerImage));
            }
        }

        private QandAMode _qandAMode;

        public QandAMode QandAMode
        {
            get { return _qandAMode; }
            set
            {
                _qandAMode = value;
                OnPropertyChanged(nameof(QandAMode));
            }
        }

        private QuestionStateConverterDTO _qVisConDTO;

        public QuestionStateConverterDTO QVisConDTO
        {
            get { return _qVisConDTO; }
            set
            {
                _qVisConDTO = value;
                OnPropertyChanged(nameof(QVisConDTO));
            }
        }

        private AnswerStateConverterDTO _aVisConDTO;

        public AnswerStateConverterDTO AVisConDTO
        {
            get { return _aVisConDTO; }
            set
            {
                _aVisConDTO = value;
                OnPropertyChanged(nameof(AVisConDTO));
            }
        }

        public bool IsConfirmed { get; set; }
        public ImageHandler ImageHandler { get; set; }
        protected OpenFileDialog openFileDialog { get; set; }
        protected int rowLimit { get; set; }

        #endregion Properties

        #region Commands

        public SaveCmd SaveFormCmd { get; set; }
        public DeleteCmd DeleteCmd { get; set; }
        public UploadImageCmd UploadImageCmd { get; set; }
        public ToggleMarkQCmd ToggleMarkQCmd { get; set; }
        public ToggleMarkACmd ToggleMarkACmd { get; set; }
        public CancelCmd CancelCmd { get; set; }
        public NewModeCmd NewModeCmd { get; set; }

        #endregion Commands

        public BaseQandAPageVM(string dbcontextName) : base(dbcontextName)
        {
            //Commands
            SaveFormCmd = new SaveCmd(this);
            DeleteCmd = new DeleteCmd(this);
            UploadImageCmd = new UploadImageCmd(this);
            ToggleMarkQCmd = new ToggleMarkQCmd(this);
            ToggleMarkACmd = new ToggleMarkACmd(this);
            CancelCmd = new CancelCmd(this);
            NewModeCmd = new NewModeCmd(this);
            //setup
            QVisConDTO = new QuestionStateConverterDTO();
            AVisConDTO = new AnswerStateConverterDTO();
            AnswerSearchTxt = "";
            QuestionSearchTxt = "";
            rowLimit = 20;
            openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            Mediator.Register(MediatorChannels.PoolingUpdate.ToString(), PoolingUpdate);
        }

        private void PoolingUpdate(object obj)
        {
            App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            {
                UpdateQuestionsList(QuestionSearchTxt);
                if (SelectedAnswer != null)
                {
                    Answer backup = new Answer();
                    backup.ShallowCopy(SelectedAnswer);
                    UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                    SelectedAnswer.ShallowCopy(backup);
                }
                else
                {
                    UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                }
            });
        }

        public abstract bool Save();

        public abstract bool Delete(BaseEntity objToDelete);

        public abstract Task<bool> UploadImage(Image imageToUpload);

        public abstract bool ToggleMarkQuestion(Question questionToMark);

        public bool ToggleMarkAnswer(Answer answer)
        {
            ClearFeedback();
            try
            {
                bool result = false;
                if (answer != null)
                {
                    if (answer.Id > 0)
                    {
                        //toggle is Resolved for question
                        answer.WasHelpfull = !answer.WasHelpfull;
                        string output = (answer.WasHelpfull) ? "helpful" : "unhelpful";
                        result = (UnitOfWork.Complete() > 0) ? true : false;
                        if (result == false)
                            ShowFeedback($"Unable to mark answer as {output}.", FeedbackType.Error);
                        else
                        {
                            UpdateAnswersList(SelectedQuestion, AnswerSearchTxt);
                            OnPropertyChanged(nameof(SelectedAnswer));
                            ShowFeedback($"Marked as {output}.", FeedbackType.Success);
                        }
                    }
                    else
                        ShowFeedback("New answers cannot be marked.", FeedbackType.Error);
                }
                else
                    ShowFeedback("No answer selected.", FeedbackType.Error);
                return result;
            }
            catch (Exception ex)
            {
                ShowFeedback(ex.Message, FeedbackType.Error);
                return false;
            }
        }

        public abstract bool Cancel();

        public abstract void EnterNewMode();

        protected abstract bool UpdateQuestionsList(string questionSearchTxt);

        protected bool UpdateAnswersList(Question sQuestion, string anserSearchTxt)
        {
            Question selectedQuestion = (Question)sQuestion;
            if (selectedQuestion != null)
                Answers = new ObservableCollection<Answer>(UnitOfWork.AnswerRepository.GetFromSearchForQuestion(selectedQuestion, anserSearchTxt));
            else
                Answers = new ObservableCollection<Answer>();
            if (Answers.Count > 0)
                return true;
            else
                return false;
        }

        protected abstract void SetHeaderMessage();
    }
}