using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemByProb
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Prob { get; set; }

        public Item(int iItemlId, string sItemName, double dItemProb)
        {
            this.Id = iItemlId;
            this.Name = sItemName;
            this.Prob = dItemProb;
        }
    }
    public class ItemCollection : IEnumerable
    {
        private Dictionary<int, Item> dicItems = new Dictionary<int, Item>();

        public Item this[int index]
        {
            get
            {
                return this.dicItems.ElementAt(index).Value;
            }
            set
            {
                this.dicItems[index] = value;
            }
        }
        public void Add(int iItemlId, string sItemName, double dItemProb)
        {
            this.dicItems[iItemlId] = new Item(iItemlId, sItemName, dItemProb);
        }
        public Item GetItemById(int iId) { return this.dicItems[iId]; }

        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<int, Item> item in this.dicItems)
            {
                yield return item.Value;
            }
        }
    }

    private ItemCollection _Items = new ItemCollection();
    public ItemCollection Items { get { return this._Items; } }

    private Item _RandomResult;
    public Item Result { get { return this._RandomResult; } }

    public void RunRandom()
    {
        int rtnItemId = -1;

        int iPercentage = 1;
        double dTotal = 0;
        foreach (Item obj in this.Items)
        {
            dTotal += obj.Prob;
        }
        dTotal = Convert.ToDouble(dTotal.ToString("F10"));
        if (dTotal > 1)
        {
            throw new Exception("Total Probability can't larger than 1.");
        }
        iPercentage = (int)Math.Pow(10, 9);

        Dictionary<int, int> dicBigSample = new Dictionary<int, int>();
        foreach (Item obj in this.Items)
        {
            dicBigSample.Add(obj.Id, (int)(obj.Prob * iPercentage));
        }

        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        int total = 0;
        foreach (KeyValuePair<int, int> obj2 in dicBigSample)
        {
            total += obj2.Value;
        }

        int rnd_num = rnd.Next(1, total + 1);
        int iProgression = 0;
        foreach (KeyValuePair<int, int> obj3 in dicBigSample)
        {
            iProgression += obj3.Value;
            if (iProgression >= rnd_num)
            {
                rtnItemId = obj3.Key;
                break;
            }
        }
        this._RandomResult = this.Items.GetItemById(rtnItemId);
    }
}