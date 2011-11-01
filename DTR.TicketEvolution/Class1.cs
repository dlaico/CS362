using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace DTR.TicketEvolution
{
    public class TicketAPI
    {

        public List<Brokerage> GetAllBrokerages()
        {
            List<Brokerage> b = GetAll<Brokerage>();
            return b;
        }

        public List<TicketGroup> GetEventTicketGroups(int event_id)
        {
            List<TicketGroup> rList = new List<TicketGroup>();
            string Response = GetAPIResponse("ticket_groups?event_id=" + event_id.ToString());

            Response = Response.Replace("\"event\":", "event_:");

            //Console.WriteLine(Response);

            JObject resultSet = JObject.Parse(Response);

            JToken results = resultSet["total_entries"];

            //int totalItems = JsonConvert.DeserializeObject<int>(results.ToString());

            List<JToken> items = resultSet["ticket_groups"].Children().ToList();

            foreach (JToken item in items) rList.Add(JsonConvert.DeserializeObject<TicketGroup>(item.ToString()));

            return rList;
        }

        public TicketGroup GetTicketGroup(int ticketgroupid)
        {
            string Response = GetAPIResponse("ticket_groups/" + ticketgroupid.ToString() + "?page=1");

            Response = Response.Replace("\"event\":", "event_:");

            TicketGroup t = JsonConvert.DeserializeObject<TicketGroup>(Response);

            return t;
        }

        //loads a supplied linked list with objects from the specified web service
        public List<T> GetAll<T>()
        {

            string typeName = typeof(T).Name.ToLower();
            if (typeName == "category") typeName = "categorie";
            string Response = GetAPIResponse(typeName + "s?page=1");

            JObject resultSet = JObject.Parse(Response);

            JToken results = resultSet["total_entries"];

            int totalItems = JsonConvert.DeserializeObject<int>(results.ToString());
            //Console.WriteLine(totalItems);

            int loopSize = (totalItems / 100) + 1;

            List<JToken> items = resultSet[typeName + "s"].Children().ToList();

            List<T> rList = new List<T>();

            for (int i = 1; i <= loopSize; i++)
            {
                Response = GetAPIResponse(typeName + "s?page=" + i.ToString());
                resultSet = JObject.Parse(Response);
                items = resultSet[typeName + "s"].Children().ToList();
                foreach (JToken item in items) rList.Add(JsonConvert.DeserializeObject<T>(item.ToString()));
            }

            return rList;

            //return new List<object>();
        }

        public string GetAPIResponse(string urlParams)
        {
            string api = "api.ticketevolution.com/" + urlParams;
            string token = "de754476c55950df4f894611dddf7c77";
            string secret = "e/sX7DLBWVvWJg9VHAbzwemeGcR9BYeio5EC0Fw8";
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bRequest = encoding.GetBytes("GET " + api);
            byte[] bSecretkey = encoding.GetBytes(secret);

            HMACSHA256 myhmacsha256 = new HMACSHA256(bSecretkey);
            myhmacsha256.ComputeHash(bRequest);
            string X_Signature = Convert.ToBase64String(myhmacsha256.Hash);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + api);
            request.Method = "GET";
            request.Accept = "application/vnd.ticketevolution.api+json; version=8";
            request.Headers.Add("X-Signature", X_Signature);
            request.Headers.Add("X-Token", token);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader loResponseStream = new
                StreamReader(response.GetResponseStream());

            string Response = loResponseStream.ReadToEnd();
            response.Close();
            return Response;
        }
    }

    public class Address
    {
        public string region { get; set; }
        public double? latitude { get; set; }
        public string country_code { get; set; }
        public string postal_code { get; set; }
        public string extended_address { get; set; }
        public string street_address { get; set; }
        public string locality { get; set; }
        public double? longitude { get; set; }
    }

    public partial class Brokerage
    {
        public string name { get; set; }
        public bool natb_member { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public string abbreviation { get; set; }

        public void print()
        {
            //Console.WriteLine("Name: " + name);
            //Console.WriteLine("Is NATB Member: " + natb_member);
            //Console.WriteLine("Last Updated: " + updated_at);
            //Console.WriteLine("URL: " + url);
            Console.WriteLine("Broker ID: " + id);
            //Console.WriteLine("Abbreviation: " + abbreviation);
        }
    }

    public partial class Category
    {
        public string name { get; set; }
        public Parent parent { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public int id { get; set; }

        public void print()
        {
            if (parent != null)
            {
                if (parent.id == 73)
                {
                    Console.WriteLine("Name: " + name);
                    //Console.WriteLine("Parent: " + parent.id);
                    Console.WriteLine("ID: " + id.ToString());
                }
            }
        }
    }

    public partial class Configuration
    {
        public string name { get; set; }
        public int? fanvenues_key { get; set; }
        public DateTime updated_at { get; set; }
        public bool primary { get; set; }
        public string url { get; set; }
        public int? id { get; set; }
        public SeatingChart seating_chart { get; set; }
        public Venue venue { get; set; }
        public int? capacity { get; set; }
        public bool general_admission { get; set; }

        public void print()
        {
            Console.WriteLine("Name: " + name);
            //venue.print();
        }
    }

    public partial class Event
    {
        public string name { get; set; }
        public int products_count { get; set; }
        public Category category { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime occurs_at { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public Performance[] performances { get; set; }
        public Venue venue { get; set; }
        public Configuration configuration { get; set; }
        public string state { get; set; }

        public void print()
        {
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Count: " + products_count);
            Console.WriteLine("Date/Time: " + occurs_at);
            if (performances != null)
            {
                foreach (var p in performances)
                {
                    p.print();
                }
            }

            //venue.print();
            //configuration.print();
        }
    }

    public partial class Office
    {
        public string name { get; set; }
        public Address address { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public string fax { get; set; }
        public string phone { get; set; }
        public bool main { get; set; }
        public string time_zone { get; set; }
        public Brokerage brokerage { get; set; }
        public string[] email { get; set; }

        public void print()
        {
            Console.WriteLine("ID: " + id);
            Console.WriteLine("Name: " + name);
            if (address == null) Console.WriteLine("Address: NO ADDRESS");
            else Console.WriteLine("Address: " + address.street_address);
        }

    }

    public partial class Parent
    {
        public Parent parent { get; set; }
        public string url { get; set; }
        public int id { get; set; }

        public void print()
        {
            Console.WriteLine("parentid: " + id.ToString());
        }
    }

    public partial class Performance
    {
        public bool primary { get; set; }
        public Performer performer { get; set; }

        public void print()
        {
            performer.print();
        }
    }

    public partial class Performer
    {
        public string name { get; set; }
        public Category category { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public Venue venue { get; set; }
        public UpcomingEvents upcoming_events { get; set; }

        public void print()
        {
            Console.WriteLine("\t\tPerformer ID: " + id);
            //category.print();
        }
    }

    public class SeatingChart
    {
        public string large { get; set; }
        public string medium { get; set; }
    }

    public partial class TicketGroup
    {
        public string public_notes { get; set; }
        public bool in_hand { get; set; }
        public string section { get; set; }
        public string row { get; set; }
        public int quantity { get; set; }
        public DateTime? in_hand_on { get; set; }
        public bool featured { get; set; }
        public int[] seats { get; set; }
        public DateTime updated_at { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public Event event_ {get;set;}
        public decimal retail_price { get; set; }
        public string type { get; set; }
        public Office office { get; set; }
        public int[] splits { get; set; }
        public bool eticket { get; set; }
        public decimal wholesale_price { get; set; }

        public void print()
        {
            Console.WriteLine("Quantity: " + quantity + "\tSection: " + section + "\tRow:" + row + "\tPrice: $" + wholesale_price);
        }
    }

    public class UpcomingEvents
    {
        public DateTime? last { get; set; }
        public DateTime? first { get; set; }
    }

    public partial class Venue
    {
        public string name { get; set; }
        public Address address { get; set; }
        public string location { get; set; }
        public string url { get; set; }
        public int id { get; set; }
        public UpcomingEvents upcoming_events { get; set; }

        public void print()
        {
            Console.WriteLine("Name: " + name);
        }
    }
}
