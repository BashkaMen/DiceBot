using DevExpress.Mvvm;
using Dice.Client.Web;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;

namespace DiceBot.Model
{
    [JsonObject]
    class DataBase : ViewModelBase
    {

        public ObservableCollection<Account> Accounts { get; set; }
        public MartingaleSettings MartingaleSettings { get; set; }


        [JsonIgnore]
        public SessionInfo CurrentSession { get; set; }

        public DataBase()
        {
            Accounts = new ObservableCollection<Account>();
            MartingaleSettings = new MartingaleSettings();
        }

        public void Save()
        {
            try
            {
                File.WriteAllText("Database.json", JsonConvert.SerializeObject(this));
            }
            catch { }
        }

        public static DataBase LoadOrDefault()
        {
            if (File.Exists("Database.json"))
            {
                return JsonConvert.DeserializeObject<DataBase>(File.ReadAllText("Database.json"));
            }
            return new DataBase();
        }


    }
}
