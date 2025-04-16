using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Order_Aggregrate;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data
{
    public  class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext context)
        {

            var AppUserData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/AppUser.json");
            var Users= JsonSerializer.Deserialize<List<AppUser>>(AppUserData);
            if(Users.Count()>0)
            {
                if(context.AppUsers.Count()==0)
                {
                   
                    foreach(var user in Users)
                    {
                       context.AppUsers.Add(user);
                    }
                    await context.SaveChangesAsync();

                }
            }


                var CategoryData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/categories.json");
                var Categories = JsonSerializer.Deserialize<List<Category>>(CategoryData);
                if (Categories.Count() > 0)
                {
                    if (context.Categories.Count() == 0)
                    {
                        foreach (var category in Categories)
                        {
                            context.Set<Category>().Add(category);
                        }
                        await context.SaveChangesAsync();
                    }
                }



            var ProductData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/Products.json");
            var Products=JsonSerializer.Deserialize<List<Product>>(ProductData);
            if(Products.Count() >0)
            {
                if(context.Products.Count()==0)
                {
                    foreach ( var product in Products )
                    {
                        context.Set<Product>().Add(product);
                    }
                    await context.SaveChangesAsync();
                }
            }



         

            var MakeReviewData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/MakeReviews.json");
            var MakeReviews=JsonSerializer.Deserialize<List<MakeReview>>(MakeReviewData);
            if(MakeReviews.Count() >0)
            {
                if(context.MakeReviews.Count()==0)
                {
                    foreach(var Make in MakeReviews)
                    {
                        context.Set<MakeReview>().Add(Make);

                    }
                    await context.SaveChangesAsync();
                }
            }

            var deliveryData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/delivery.json");
            var deliveries=JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
            if(deliveries.Count() >0)
            {
                if(context.deliveryMethods.Count()==0)
                {
                    foreach(var delivery in deliveries)
                    {
                       if(delivery.Id==null)
                        {
                            delivery.Id = Guid.NewGuid().ToString();
                        }
                        context.Set<DeliveryMethod>().Add(delivery);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Adel",
                    Email = "AboMousaOfficial@gmail.com",
                    UserName = "Ahmed.Adel",
                    PhoneNumber = "011223365"
                };
                //take the user and start Create
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

        }   
    }
}
