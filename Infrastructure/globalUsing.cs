//Add using for global use
global using Domain.Models;
global using DomainServices.RepoInterfaces;
global using ApplicationServices.Interfaces;

//Examples for coding:
//The ?? is there to give a second return for when the initial value is null
//return View(new string[] { products[0]?.Name ?? "No Value" });

//The ! is used to tell the program that you are certain that there won't be a null
//This can be used to get rid of unjust errors
//return View(new string[] { products[0]!.Name });

//String interpolation
//Tip string interpolation supports the string format specifiers,
//which can be applied within holes, so $"Price: {price:C2}" would format
//the price value as a currency value with two decimal digits, for example.
//$"Name: {products[0]?.Name}, Price: { products[0]?.Price:C2 }"

//Use switch statement to check if a answer is a certaint type with case
//You can also add a when to it
//decimal total = 0;
//for (int i = 0; i < data.Length; i++)
//{
//    switch (data[i])
//    {
//If it is a decimal create decimalValue
//        case decimal decimalValue:
//            total += decimalValue;
//            break;
//If it is a int that is > 50 create intValue 
//        case int intValue when intValue > 50:
//            total += intValue;
//            break;
//    }
//}

//yield return means it wil cuntinu after the return and make a list of responses
//the return stops when a yield break is reached, or the code blok has ended

//A example of using a function as an object
//public static IEnumerable<Product?> Filter(
//this IEnumerable<Product?> productEnum,
//Func<Product?, bool> selector)
//Product is what the function needs and bool is what it returns
//{
//    foreach (Product? prod in productEnum)
//    {
//        if (selector(prod))
//        {
//            yield return prod;
//        }
//    }
//}

//Creating the function
//public (delegate) bool Func<Product?, bool> checkPrice(Product? p){
//    price = p?.Price ?? 0;
//    if(price >= 20){ return
//        price
//    }
//}

//Caling the function
//.Filter(checkPrice(p))

//A example of using a lamba expresion as an object
//.Filter(p => (p?.Price ?? 0) >= 20)

//Use non hard coded names in front of the answer
//namespace LanguageFeatures.Controllers
//{
//    public class HomeController : Controller
//    {
//        public ViewResult Index()
//        {
//            var products = new[] {
// new { Name = "Kayak", Price = 275M },
// new { Naam = "Lifejacket", Prijs = 48.95M },
// new { Name = "Soccer ball", Price = 19.50M },
// new { Name = "Corner flag", Price = 34.95M }
// };
//            return View(products.Select(p =>
//            $"{nameof(p.Name)}: {p.Name}, {nameof(p.Price)}: {p.Price}"));
//The {nameof(p.Name)} writes Name and the {nameof(p.Price)} writes Price
//So for the Kayak it will look like: Name: Kayak, Price: 275
//And for the lifejacket: Naam: Lifejacket, Prijs: 48.95
//        }
//    }
//}

//Compact if statement
//int eenGetal = (User input)
//antwoord = eenGetal == 13 ? eenGetal : -1;
//If eeGetal == 13 then atwoord will be eenGetal, else it will be -1