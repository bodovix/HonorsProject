using HonorsProject.Model.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HonorsProject.ViewModel.Commands.IComands
{
    public interface IUploadImageCmd
    {
        UploadImageCmd UploadImageCmd { get; set; }
        bool UploadImage(Image imageToUpload);
    }
}
