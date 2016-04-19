using System.Linq;
using laundry.Models.DB;

namespace laundry.ViewModels
{
    public class ManageCateogry
    {

        //TODO: Add New MainCategory
        public void AddNewMainCat(AddNewMainCategory itMaCa)
        {
            using (LundryDbContext db = new LundryDbContext())
            {
                ItemMainCategory mainCat = new ItemMainCategory();
                mainCat.catName = itMaCa.catName;
                db.ItemMainCategories.Add(mainCat);
                db.SaveChanges();
            }
        }
        //TODO: Check to see if the Main Category is added before
        public bool isMainCateogryExists(string catname)
        {
            using (LundryDbContext db = new LundryDbContext())
            {
                bool getCat = db.ItemMainCategories.Where(x => x.catName.Equals(catname)).Any();
                if (getCat == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

    }
}
