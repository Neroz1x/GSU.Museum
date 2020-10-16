using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class ExhibitGalleryViewModel : BaseViewModel
    {
        #region Fields
        public ObservableCollection<PhotoInfoDTO> Photos { get; }

        public ExhibitDTO Exhibit;

        // Title of the page
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (value != _title)
                {
                    _title = value;
                }
                OnPropertyChanged(nameof(Title));
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibit">Exhibit to display</param>
        /// <param name="navigation">Instance of navigation</param>
        /// <param name="navigation">Instance of navigation</param>
        public ExhibitGalleryViewModel(ExhibitDTO exhibit, INavigation navigation) : base(navigation)
        {
            Exhibit = exhibit;
            Photos = new ObservableCollection<PhotoInfoDTO>();
        }

        #region Methods
        public void FillPage()
        {
            Photos.Clear();
            foreach(var photo in Exhibit.Photos)
            {
                Photos.Add(photo);
            }
            Title = Exhibit.Title;
        }
        #endregion

    }
}
