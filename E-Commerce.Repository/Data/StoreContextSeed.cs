using E_Commerce.Core.Entities;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Order_Aggregrate;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Text.Json;

namespace E_Commerce.Repository.Data
{
    public class StoreContextSeed
    {
        public async static Task SeedAsync(StoreContext context)
        {

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
            var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
            if (Products.Count() > 0)
            {
                if (context.Products.Count() == 0)
                {
                    foreach (var product in Products)
                    {
                        context.Set<Product>().Add(product);
                    }
                    await context.SaveChangesAsync();
                }
            }

            //var MakeReviewData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/MakeReviews.json");
            //var MakeReviews = JsonSerializer.Deserialize<List<Review>>(MakeReviewData);
            //if (MakeReviews.Count() > 0)
            //{
            //    if (context.MakeReviews.Count() == 0)
            //    {
            //        foreach (var Make in MakeReviews)
            //        {
            //            context.Set<Review>().Add(Make);

            //        }
            //        await context.SaveChangesAsync();
            //    }
            //}

            var deliveryData = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/delivery.json");
            var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
            if (deliveries.Count() > 0)
            {
                if (context.deliveryMethods.Count() == 0)
                {
                    foreach (var delivery in deliveries)
                    {
                        if (delivery.Id == null)
                        {
                            delivery.Id = Guid.NewGuid().ToString();
                        }
                        context.Set<DeliveryMethod>().Add(delivery);
                    }
                    await context.SaveChangesAsync();
                }
            }

            var roles = await context.Roles.ToListAsync();
            if (!roles.Any())
            {
                var rolesDataFile = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/roles.json");
                var rolesData = JsonSerializer.Deserialize<List<IdentityRole>>(rolesDataFile);
                foreach (var role in rolesData)
                {
                    if (!await context.Roles.AnyAsync(r => r.Name == role.Name))
                    {
                        await context.Roles.AddAsync(role);
                    }
                }
                await context.SaveChangesAsync();
            }

            var users = await context.Users.ToListAsync();
            if (!users.Any())
            {
                var usersDataFile = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/users.json");
                var usersData = JsonSerializer.Deserialize<List<AppUser>>(usersDataFile);
                foreach (var user in usersData)
                {
                    if (!await context.Users.AnyAsync(r => r.Id == user.Id))
                    {
                        await context.Users.AddAsync(user);
                        if(user.Role == Core.Enums.AppRole.User)
                        {
                            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == user.Role.ToString());
                            await context.UserRoles.AddAsync(new()
                            {
                                RoleId = role.Id,
                                UserId = user.Id
                            });
                        }else if (user.Role == Core.Enums.AppRole.Admin)
                        {
                            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == user.Role.ToString());
                            await context.UserRoles.AddAsync(new()
                            {
                                RoleId = role.Id,
                                UserId = user.Id
                            });
                        }
                    }
                }
                //var userRolesDataFile = File.ReadAllText("../E-Commerce.Repository/Data/DataSeeding/user-roles.json");
                //var userRolesData = JsonSerializer.Deserialize<List<IdentityUserRole<string>>>(usersDataFile);
                //foreach (var role in userRolesData)
                //{
                //    await context.UserRoles.AddAsync(role);
                //}
                await context.SaveChangesAsync();
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
