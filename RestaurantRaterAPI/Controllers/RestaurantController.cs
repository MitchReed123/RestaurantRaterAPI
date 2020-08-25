using RestaurantRaterAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace RestaurantRaterAPI.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        //create
        [HttpPost]
        public async Task<IHttpActionResult> PostRestaurant(Restaurant model)
        {
            if (model is null)
            {
                return BadRequest("Request body cant be empty");
            }
            if (ModelState.IsValid)
            {
                _context.Restaurants.Add(model);
                await _context.SaveChangesAsync();

                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        //read by id

        [HttpGet]
        public async Task<IHttpActionResult> GetById(int id)
        {
            //List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();

            //foreach (Restaurant item in restaurants)
            //{
            //    if(id == item.Id)
            //    {
            //        return Ok(item);
            //    }

            //}
            //return BadRequest("nope");

            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }


        //read by all
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();

            return Ok(restaurants);
        }
        //put
        [HttpPut]
        public async Task<IHttpActionResult> UpdateItem([FromUri]int id, [FromBody]Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                Restaurant restauranttwo = await _context.Restaurants.FindAsync(id);
                if(restauranttwo != null)
                {
                    restauranttwo.Name = restaurant.Name;
                    restauranttwo.Rating = restaurant.Rating;

                    await _context.SaveChangesAsync();
                    return Ok(restauranttwo);
                }
            }
            return BadRequest(ModelState);
        }

        //delete
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteItem([FromUri]int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);

            if(await _context.SaveChangesAsync() == 1)
            {
                return Ok("Deleted");
            }

            return InternalServerError();
        }

    }
}
