using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using TVItems.DAta;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var xmlItems = GetItemsFromXml("tv.xml");
            var dbItems = GetItemsFromDb();
            var repo = new TVItemsRepo(Properties.Settings.Default.ConStr);
            repo.ClearChangeLogs();
            int counter = 0;
            foreach (Item xmlItem in xmlItems)
            {
                counter++;
                if (counter % 1000 == 0)
                {
                    Console.WriteLine("Chuggin along... " + counter);
                }

                TVItem dbItem = dbItems.FirstOrDefault(i => i.ItemNumber == xmlItem.ItemNumber);
                if (dbItem == null)
                {
                    repo.AddChangeLog(new ChangeLog
                    {
                        ItemNumber = xmlItem.ItemNumber,
                        Type = "New Item Added"
                    });
                    repo.AddItem(ConvertToDbItem(xmlItem));
                }
                else
                {
                    IEnumerable<ChangeLog> changes = GetChanges(xmlItem, dbItem);
                    if (changes.Any())
                    {
                        foreach (ChangeLog change in changes)
                        {
                            repo.AddChangeLog(change);
                        }
                        repo.Update(ConvertToDbItem(xmlItem));
                    }
                }
            }

            foreach (TVItem dbItem in dbItems)
            {
                Item xmlItem = xmlItems.FirstOrDefault(i => i.ItemNumber == dbItem.ItemNumber);
                if (xmlItem == null)
                {
                    repo.Delete(dbItem.ItemNumber);
                    repo.AddChangeLog(new ChangeLog
                    {
                        ItemNumber = dbItem.ItemNumber,
                        Type = "Item Removed"
                    });
                }
            }

            Console.WriteLine("Done");
            Console.ReadKey(true);
        }

        private static IEnumerable<Item> GetItemsFromXml(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Root));
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                var root = serializer.Deserialize(stream) as Root;
                return root.Truserv.Items;
            }
        }

        private static IEnumerable<TVItem> GetItemsFromDb()
        {
            return new TVItemsRepo(Properties.Settings.Default.ConStr).GetAll();
        }

        private static TVItem ConvertToDbItem(Item item)
        {
            return new TVItem
            {
                ItemNumber = item.ItemNumber,
                Length = item.Length,
                ClassCode = item.ClassCode,
                Height = item.Height,
                LongDescription = item.LongDescription,
                MemberCost = item.MemberCost,
                Model = item.Model,
                ShortDescription = item.ShortDescription,
                SubClassCode = item.SubclassCode,
                Upc = item.Upc,
                VendorName = item.VendorName,
                Weight = item.Weight,
                Width = item.Width
            };
        }

        private static ChangeLog GetChangeLog(int itemNumber, string column, string from, string to)
        {
            return new ChangeLog
            {
                ItemNumber = itemNumber,
                Type = column,
                From = from,
                To = to
            };
        }

        private static IEnumerable<ChangeLog> GetChanges(Item xmlItem, TVItem dbItem)
        {
            List<ChangeLog> changes = new List<ChangeLog>();
            if (xmlItem.ClassCode != dbItem.ClassCode)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "ClassCode", dbItem.ClassCode.ToString(),
                    xmlItem.ClassCode.ToString());
                changes.Add(change);
            }
            if (xmlItem.Height != dbItem.Height)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "Height", dbItem.Height.ToString(),
                   xmlItem.Height.ToString());
                changes.Add(change);
            }
            if (xmlItem.Weight != dbItem.Weight)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "Weight", dbItem.Weight.ToString(),
                   xmlItem.Weight.ToString());
                changes.Add(change);
            }
            if (xmlItem.Width != dbItem.Width)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "Width", dbItem.Width.ToString(),
                   xmlItem.Width.ToString());
                changes.Add(change);
            }

            if (xmlItem.Length != dbItem.Length)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "Length", dbItem.Length.ToString(),
                   xmlItem.Length.ToString());
                changes.Add(change);
            }

            if (xmlItem.MemberCost != dbItem.MemberCost)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "MemberCost", dbItem.MemberCost.ToString(),
                   xmlItem.MemberCost.ToString());
                changes.Add(change);
            }

            if (xmlItem.SubclassCode != dbItem.SubClassCode)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "SubClassCode", dbItem.SubClassCode.ToString(),
                   xmlItem.SubclassCode.ToString());
                changes.Add(change);
            }

            if (xmlItem.LongDescription != dbItem.LongDescription)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "LongDescription", dbItem.LongDescription.ToString(),
                   xmlItem.LongDescription.ToString());
                changes.Add(change);
            }

            if (xmlItem.ShortDescription != dbItem.ShortDescription)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "ShortDescription", dbItem.ShortDescription.ToString(),
                   xmlItem.ShortDescription.ToString());
                changes.Add(change);
            }

            if (xmlItem.Model != dbItem.Model)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "Model", dbItem.Model.ToString(),
                   xmlItem.Model.ToString());
                changes.Add(change);
            }

            if (xmlItem.Upc != dbItem.Upc)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "Upc", dbItem.Upc.ToString(),
                   xmlItem.Upc.ToString());
                changes.Add(change);
            }

            if (xmlItem.VendorName != dbItem.VendorName)
            {
                var change = GetChangeLog(xmlItem.ItemNumber, "VendorName", dbItem.VendorName.ToString(),
                   xmlItem.VendorName.ToString());
                changes.Add(change);
            }

            return changes;
        }
    }



    [XmlRoot("Root")]
    public class Root
    {
        [XmlElement("Truserv")]
        public Truserv Truserv { get; set; }
    }

    public class Truserv
    {
        [XmlElement("Item")]
        public Item[] Items { get; set; }
    }

    public class Item
    {
        [XmlAttribute("item_nbr")]
        public int ItemNumber { get; set; }

        [XmlAttribute("member_cost")]
        public decimal MemberCost { get; set; }

        [XmlAttribute("short_description")]
        public string ShortDescription { get; set; }

        [XmlAttribute("class_code")]
        public int ClassCode { get; set; }

        [XmlAttribute("subclass_code")]
        public int SubclassCode { get; set; }

        [XmlAttribute("vendor_name")]
        public string VendorName { get; set; }

        [XmlAttribute("long_description")]
        public string LongDescription { get; set; }

        [XmlAttribute("upc")]
        public string Upc { get; set; }

        [XmlAttribute("weight")]
        public decimal Weight { get; set; }

        [XmlAttribute("length")]
        public decimal Length { get; set; }

        [XmlAttribute("width")]
        public decimal Width { get; set; }

        [XmlAttribute("height")]
        public decimal Height { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

    }

}