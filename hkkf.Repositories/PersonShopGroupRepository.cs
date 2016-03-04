using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using hkkf.Common;
using hkkf.Models;
using JieNuo.Data;
using NHibernate.Linq;

namespace hkkf.Repositories
{

    public class PersonShopGroupRepository : NHibernateRepository<PersonShopGroup, int>
    {
        ShopGroupRepository ShopGroupRepo = new ShopGroupRepository();
        private ShopGroupDetailRepository shopGroupDetailRepo = new ShopGroupDetailRepository();//
        PinFenRepository pinFenRepo = new PinFenRepository();
        private ShopRepository shopRepo = new ShopRepository();//

        //更新班组可以由哪些客服来做，这个更新是系统根据店铺客服对应表、班组店铺对应表自动更新的，不是逐条加入的
        public void updatePersonShopGroup()
        {
            PersonShopGroupRepository PersonShopGroupRepo = new PersonShopGroupRepository();
            List<PinFen> listPinFen = pinFenRepo.GetAll().ToList();
            List<ShopGroupDetails> listShopGroupDetail = shopGroupDetailRepo.GetAll().ToList();
            List<ShopGroups> listShopGroup = ShopGroupRepo.GetAll().ToList();

            //清空原来数据
            List<PersonShopGroup> listPersonShopGroup = PersonShopGroupRepo.GetAll().ToList();
            foreach (var PSP in listPersonShopGroup)
            {
                PersonShopGroupRepo.Delete(PSP);
            }
            //(1)取出第一个班组来，(2)然后取出该班组的第一个店铺，(3)再取出能够做该店铺的客服，再取出第二个店铺的客服，所以店铺的客服如果有重复的，那么插入。
            foreach (var ShopGroup in listShopGroup)
            {
                var loacalListShopGroupDetail = listShopGroupDetail.Where(it => it._ShopGroup == ShopGroup);
                //根据店铺LIST去取客服List，判断有无重复的客服，也就是班组中的每个店铺都可以做的客服。有几个，插入几个。
                List<User> listUser = new List<User>();
                // foreach (var shopGroupDetail in loacalListShopGroupDetail)
                for (int k = 0; k < loacalListShopGroupDetail.Count(); k++)
                {
                    List<User> LocalListUser = new List<User>();
                    var localListFinFen = listPinFen.Where(it => it._shop == loacalListShopGroupDetail.ElementAt(k)._Shop);//第一个店铺里面有哪些人找出来, 第二个店铺找出来，都重复的加进去
                    //只要有一个店铺的客服没有分配，那么该班组的客服肯定没有,就去找下一个店铺
                    if (localListFinFen.Count() == 0)
                    {
                        goto last;
                    }
                    //每个店铺都有客服可以做，那么先插入第一个店铺的客服，再判断第二个店铺的客服是否有重复，重复的留下，不重复的删除。
                    foreach (var LocalPinFen in localListFinFen)
                    {
                        LocalListUser.Add(LocalPinFen._user);
                    }
                    if (k == 0)//只是第一个店铺插入,第二个就不能插入了。
                    {
                        foreach (var LocalUser in LocalListUser)
                        {
                            listUser.Add(LocalUser);
                        }
                    }
                    //比较LISTUSER和localListPinFen，重复的留下，不重复的删除
                    List<User> deleteListUser = new List<User>();
                    foreach (var user in listUser)
                    {
                        if (localListFinFen.Where(it => it._user == user).Count() == 0)
                        {
                            //在重复的留下，不重复的删除
                            deleteListUser.Add(user);
                        }
                    }
                    //删除哪些USER
                    foreach (var deleteUser in deleteListUser)
                    {
                        listUser.Remove(deleteUser);
                    }
                }
                foreach (var _user in listUser)
                {
                    PersonShopGroup PersonShopGroup = new PersonShopGroup();
                    PersonShopGroup._ShopGroups = ShopGroup;
                    PersonShopGroup._User = _user;
                    PersonShopGroup.UpdateTime = DateTime.Today;
                    PersonShopGroupRepo.Save(PersonShopGroup);
                }
            last: ;//操作下一个店铺
            }                               
        }
        public string checkPersonShopGroupValid(ShopGroups shopGroup)
        {
            string strResult = "";
            //坚持班组在班组客服表中有无数据，如果有数据，则返回有效，如果没有数据，返回班组没有客服
            //List<ShopGroups> listShopGroup = this.ShopGroupRepo.GetAll().ToList();
            int intCount= this.GetAll()
                .Where(it=>it._ShopGroups==shopGroup)
                .Count();

            if (intCount == 0)
            {
                strResult = "班组"+shopGroup.ShopGroupName.Trim()+"没有客服,请检查后再排班！";
                return strResult;
             }
            strResult = "班组客服有效";
            return strResult;
        }

    }
}
