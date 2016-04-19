using System.Linq;
using laundry.Models.DB;

namespace laundry.ViewModels
{
    public class ManageDisc
    {
        private LundryDbContext db = new LundryDbContext();

        //TODO:check for Allbra is true or false
        public bool isAllbraActive()
        {
            bool getIsActive = db.Discs.Where(x => x.Allbra.Equals(true)).Any();
            return getIsActive;
        }

        //TODO:Add Discount-- Get is branche is active throught its brncke id and isActive(true/false)
        //here am saying if the same branche id, and isactive has been added before then reject the addition by returning TRUE

        public bool getBranchById(int? BrId, bool st)
        {
            //try to find if there is any branche with the same branch id and is active
            bool gBra = db.Discs.Where(x => x.Branche == BrId && x.status == st).Any();
            return gBra;
        }

        //TODO:Edit Discount-- Get is branche is active throught its brancke id, and isActive(true/false) plus the overall Id of the row, 
        //this is important because during the edition process I don't wnat to reject the same discout if i just modified the precentatge or the time
        //here am saying if the same discuout, with the same branche id, and isactive and id has been modfied then don' reject them,by returning FALSE, but if one of these parameter is differect then find if there is a mache and if so reject the edtion by returing TRUE
        public bool getBranchById(int? BrId, bool st, int id)
        {
            if (BrId == null)//check to see if the all branche is checked or not
            {
                
                return false;
            }
            else
            {
                //try to find if there is any branche with the same branch id and is active
                bool gBra = db.Discs.Where(x => x.Branche == BrId && x.status == st && x.Id == id).Any();
                if (gBra)
                {
                    return true;
                }
                else
                {
                    if(isAllbraActive())//here we are going to check to see if there is a grand discount is active if so we can't add a private discount on branche lundary
                    {
                        return true;
                    }
                    else
                    {
 return false;
                    }
                   
                }
                // bool getBra = db.Discs.Where(x => x.Branche.Equals(BrId)).Any();
            }

        }

        //TODO: Disable all the Discount.This may used in case we choosed discount fro All Branche, and we need to make all other discound in active
        public void DisableAll()
        {
            using (var db = new LundryDbContext())
            {
                db.Discs
                  .Where(x => x.status == true)
                  .ToList()
                  .ForEach(a => a.status = false);

                db.SaveChanges();
            }
            //var disall = db.Discs.Where(x => x.status == true).ToList().ForEach(x=>x.status= false);
        }

    }
}
