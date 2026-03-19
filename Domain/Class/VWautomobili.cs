using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.Class
{
    public class VWautomobili : INotifyPropertyChanged
    {
        public int cena {  get; set; } 
        public string name { get; set; } //naziv i hyperlink
        public string slikaPath { get; set; }  // putanja do slike
        public string RTFpath { get; set; } //rich text file path
        public string date { get; set; }   //datum

        private bool _isSelected;

        [XmlIgnore]
        public bool IsSelected      //da li se checkovalo
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }    

        public VWautomobili() { }

        public VWautomobili(int cena, string name, string slikaPath, string rTFpath,string date)
        {
            this.cena = cena;
            this.name = name;
            this.slikaPath = slikaPath;
            RTFpath = rTFpath;
            this.date = DateTime.Now.ToString("dd.MM.yyyy.");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
