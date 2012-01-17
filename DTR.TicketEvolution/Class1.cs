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

        public TicketGroup GetTicketGroupSandbox(int ticketgroupid)
        {
            string Response = GetAPIResponseTest("ticket_groups/" + ticketgroupid.ToString() + "?page=1");

            Response = Response.Replace("\"event\":", "event_:");

            TicketGroup t = JsonConvert.DeserializeObject<TicketGroup>(Response);

            return t;
        }

        public List<Event> GetEventsSandbox()
        {
            string Response = GetAPIResponseTest("events?per_page=100?page=1");
            JObject EventSet = JObject.Parse(Response);
            List<JToken> items = EventSet["events"].Children().ToList();
            List<Event> e = new List<Event>();
            foreach (JToken item in items)
            {
                Event event_ = JsonConvert.DeserializeObject<Event>(item.ToString());
                e.Add(event_);
            }
            return e;
        }

        public List<Office> GetOfficesSandbox()
        {
            string Response = GetAPIResponseTest("offices?per_page=100?page=1");
            JObject OfficeSet = JObject.Parse(Response);
            List<JToken> items = OfficeSet["offices"].Children().ToList();
            List<Office> o = new List<Office>();
            foreach (JToken item in items)
            {
                Office office = JsonConvert.DeserializeObject<Office>(item.ToString());
                o.Add(office);
            }
            return o;
        }

        public List<User> GetUsersByOfficeIdSandbox()
        {
            string Response = GetAPIResponseTest("users?office_id=53");
            JObject UserSet = JObject.Parse(Response);
            List<JToken> items = UserSet["users"].Children().ToList();
            List<User> u = new List<User>();
            foreach (JToken item in items)
            {
                User user = JsonConvert.DeserializeObject<User>(item.ToString());
                u.Add(user);
            }
            return u;
        }

        public List<Office> GetOfficesSandboxByBrokerage()
        {
            string Response = GetAPIResponseTest("offices?brokerage_id=200?per_page=100?page=1");
            JObject OfficeSet = JObject.Parse(Response);
            List<JToken> items = OfficeSet["offices"].Children().ToList();
            List<Office> o = new List<Office>();
            foreach (JToken item in items)
            {
                Office office = JsonConvert.DeserializeObject<Office>(item.ToString());
                o.Add(office);
            }
            return o;
        }

        public Office GetOfficeByIDSandbox(int officeid)
        {
            string Response = GetAPIResponseTest("offices/" + officeid.ToString());
            Office office = JsonConvert.DeserializeObject<Office>(Response);
            return office;
        }

        public List<Account> GetEvoPayAccountsSandbox()
        {
            string Response = GetAPIResponseTest("accounts?per_page=100?page=1");
            JObject AccountsSet = JObject.Parse(Response);
            List<JToken> items = AccountsSet["accounts"].Children().ToList();
            List<Account> a = new List<Account>();
            foreach (JToken item in items)
            {
                Account account = JsonConvert.DeserializeObject<Account>(item.ToString());
                a.Add(account);
            }
            return a;
        }

        public List<TicketGroup> GetTicketGroupsByEventSandbox(int eventid)
        {
            List<TicketGroup> rList = new List<TicketGroup>();
            string Response = GetAPIResponseTest("ticket_groups?event_id=" + eventid.ToString());

            Response = Response.Replace("\"event\":", "event_:");

            //Console.WriteLine(Response);

            JObject resultSet = JObject.Parse(Response);

            JToken results = resultSet["total_entries"];

            //int totalItems = JsonConvert.DeserializeObject<int>(results.ToString());

            List<JToken> items = resultSet["ticket_groups"].Children().ToList();

            foreach (JToken item in items) rList.Add(JsonConvert.DeserializeObject<TicketGroup>(item.ToString()));

            return rList;
        }

        //add one client at a time
        public Client AddClient(string name, int office_id)
        {
            string temp = "{\"clients\":[{";
            if (name != "")
                temp = temp + "\"name\":\"" + name + "\",";
            if (office_id != 0)
                temp = temp + "\"office_id\":" + office_id.ToString() + ",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("clients?", temp);

            JObject ClientSet = JObject.Parse(Response);
            List<JToken> items = ClientSet["clients"].Children().ToList();
            List<Client> c = new List<Client>();
            foreach (JToken item in items)
            {
                Client client = JsonConvert.DeserializeObject<Client>(item.ToString());
                c.Add(client);
            }
            if (c.Count > 0)
                return c[0];
            else
                return null;
        }

        public Client UpdateClient(int id, string name, int office_id)
        {
            string temp = "{";
            if (name != "")
                temp = temp + "\"name\":\"" + name + "\",";
            if (office_id != 0)
                temp = temp + "\"office_id\":" + office_id.ToString() + ",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("clients/" + id.ToString() + "?", temp);
            Client c = JsonConvert.DeserializeObject<Client>(Response);
            return c;
        }

        public Client GetClient(int id)
        {
            string Response = GetAPIResponseTest("clients/" + id.ToString());      
            Client client = new Client();
            client = JsonConvert.DeserializeObject<Client>(Response);
            return client;
        }

        public List<Client> GetAllClients()
        {
            List<Client> c = new List<Client>();
            string Response = GetAPIResponseTest("clients?page=1?per_page=1");
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("clients?per_page=" + per_page.ToString() + "?" + "page=" + (i+1).ToString());
                page = JObject.Parse(Response);
                List<JToken> items = page["clients"].Children().ToList();
                foreach (JToken item in items)
                {
                    Client client = JsonConvert.DeserializeObject<Client>(item.ToString());
                    c.Add(client);
                }
            }
            return c;
        }

        public List<Client> GetAllClientsByOffice(int office_id)
        {
            List<Client> c = new List<Client>();
            string Response = GetAPIResponseTest("clients?office_id=" + office_id.ToString() + "?page=1?per_page=1");
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("clients?office_id=" + office_id.ToString() + "?per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                page = JObject.Parse(Response);
                List<JToken> items = page["clients"].Children().ToList();
                foreach (JToken item in items)
                {
                    Client client = JsonConvert.DeserializeObject<Client>(item.ToString());
                    c.Add(client);
                }
            }
            return c;
        }

        //add one client address at a time
        public Address AddClientAddress(int id, string name, string company, string street_address, string extended_address, string locality, string region, string postal_code, string country_code, string label)
        {
            string temp = "{\"addresses\":[{";
            if (name != "")
                temp = temp + "\"name\":\"" + name + "\",";
            if (company != "")
                temp = temp + "\"company\":\"" + company + "\",";
            if (street_address != "")
                temp = temp + "\"street_address\":\"" + street_address + "\",";
            if (extended_address != "")
                temp = temp + "\"extended_address\":\"" + extended_address + "\",";
            if (locality != "")
                temp = temp + "\"locality\":\"" + locality + "\",";
            if (region != "")
                temp = temp + "\"region\":\"" + region + "\",";
            if (postal_code != "")
                temp = temp + "\"postal_code\":\"" + postal_code + "\",";
            if (country_code != "")
                temp = temp + "\"country_code\":\"" + country_code + "\",";
            if (label != "")
                temp = temp + "\"label\":\"" + label + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("clients/" + id.ToString() + "/addresses?", temp);
            JObject AddressSet = JObject.Parse(Response);
            List<JToken> items = AddressSet["addresses"].Children().ToList();
            List<Address> a = new List<Address>();
            foreach (JToken item in items)
            {
                Address address = JsonConvert.DeserializeObject<Address>(item.ToString());
                a.Add(address);
            }
            if (a.Count > 0)
                return a[0];
            else
                return null;
        }

        public Address UpdateClientAddress(int clientid, int addressid, string name, string company, string street_address, string extended_address, string locality, string region, string postal_code, string country_code, string label)
        {
            string temp = "{";
            if (name != "")
                temp = temp + "\"name\":\"" + name + "\",";
            if (company != "")
                temp = temp + "\"company\":\"" + company + "\",";
            if (street_address != "")
                temp = temp + "\"street_address\":\"" + street_address + "\",";
            if (extended_address != "")
                temp = temp + "\"extended_address\":\"" + extended_address + "\",";
            if (locality != "")
                temp = temp + "\"locality\":\"" + locality + "\",";
            if (region != "")
                temp = temp + "\"region\":\"" + region + "\",";
            if (postal_code != "")
                temp = temp + "\"postal_code\":\"" + postal_code + "\",";
            if (country_code != "")
                temp = temp + "\"country_code\":\"" + country_code + "\",";
            if (label != "")
                temp = temp + "\"label\":\"" + label + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("clients/" + clientid.ToString() + "/addresses/" + addressid.ToString() + "?", temp);
            Address a = JsonConvert.DeserializeObject<Address>(Response);
            return a;
        }

        public Address GetClientAddress(int clientid, int addressid)
        {
            string Response = GetAPIResponseTest("clients/" + clientid.ToString() + "/addresses/" + addressid.ToString());
            Address a = JsonConvert.DeserializeObject<Address>(Response);
            return a;
        }

        public List<Address> GetAllClientAddresses(int id) //clientid
        {
            string Response = GetAPIResponseTest("clients/" + id.ToString() + "/addresses?per_page=1?page=1");
            List<Address> a = new List<Address>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("clients/" + id.ToString() + "/addresses?" + "per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                page = JObject.Parse(Response);
                List<JToken> items = page["addresses"].Children().ToList();
                foreach (JToken item in items)
                {
                    Address address = JsonConvert.DeserializeObject<Address>(item.ToString());
                    a.Add(address);
                }
            }
            return a;
        }

        //add one email address at a time
        public Email_Address AddClientEmailAddress(int id, string address, string label)
        {
            string temp = "{\"email_addresses\":[{\"address\":\"" + address + "\",";
            if (label != "")
                temp = temp + "\"label\":\"" + label + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("clients/" + id.ToString() + "/email_addresses?", temp);
            JObject EmailAddressSet = JObject.Parse(Response);
            List<JToken> items = EmailAddressSet["email_addresses"].Children().ToList();
            List<Email_Address> ea = new List<Email_Address>();
            foreach (JToken item in items)
            {
                Email_Address email_address = JsonConvert.DeserializeObject<Email_Address>(item.ToString());
                ea.Add(email_address);
            }
            if (ea.Count > 0)
                return ea[0];
            else
                return null;
        }

        public Email_Address UpdateClientEmailAddress(int clientid, string address, int emailaddressid, string label)
        {
            string temp = "{\"address\":\"" + address + "\",";
            if (label != "")
                temp = temp + "\"label\":\"" + label + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("clients/" + clientid.ToString() + "/email_addresses/" + emailaddressid.ToString() + "?", temp);
            Email_Address ea = JsonConvert.DeserializeObject<Email_Address>(Response);
            return ea;
        }

        public Email_Address GetClientEmailAddress(int clientid, int emailaddressid)
        {
            string Response = GetAPIResponseTest("clients/" + clientid.ToString() + "/email_addresses/" + emailaddressid.ToString());
            Email_Address ea = JsonConvert.DeserializeObject<Email_Address>(Response);
            return ea;
        }

        public List<Email_Address> GetAllClientEmailAddresses(int id)
        {
            string Response = GetAPIResponseTest("clients/" + id.ToString() + "/email_addresses?per_page=1?page=1");
            List<Email_Address> ea = new List<Email_Address>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("clients/" + id.ToString() + "/email_addresses?" + "per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                page = JObject.Parse(Response);
                List<JToken> items = page["email_addresses"].Children().ToList();
                foreach (JToken item in items)
                {
                    Email_Address email_address = JsonConvert.DeserializeObject<Email_Address>(item.ToString());
                    ea.Add(email_address);
                }
            }
            return ea;
        }

        public Phone_Number AddClientPhoneNumber(int id, string number, string country_code, string extension, string label)
        {
            string temp = "{\"phone_numbers\":[{";
            temp = temp + "\"number\":\"" + number + "\",";
            if (country_code != "")
                temp = temp + "\"country_code\":\"" + country_code + "\",";
            if (extension != "")
                temp = temp + "\"extension\":\"" + extension + "\",";
            if (label != "")
                temp = temp + "\"label\":\"" + label + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("clients/" + id.ToString() + "/phone_numbers?", temp);
            JObject PhoneNumberSet = JObject.Parse(Response);
            List<JToken> items = PhoneNumberSet["phone_numbers"].Children().ToList();
            List<Phone_Number> p = new List<Phone_Number>();
            foreach (JToken item in items)
            {
                Phone_Number phone_number = JsonConvert.DeserializeObject<Phone_Number>(item.ToString());
                p.Add(phone_number);
            }
            if (p.Count > 0)
                return p[0];
            else
                return null;
        }

        public Phone_Number UpdateClientPhoneNumber(int clientid, int phonenumberid, string number, string country_code, string extension, string label)
        {
            string temp = "{\"number\":\"" + number + "\",";
            temp = temp + "\"number\":\"" + number + "\",";
            if (country_code != "")
                temp = temp + "\"country_code\":\"" + country_code + "\",";
            if (extension != "")
                temp = temp + "\"extension\":\"" + extension + "\",";
            if (label != "")
                temp = temp + "\"label\":\"" + label + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("clients/" + clientid.ToString() + "/phone_numbers/" + phonenumberid.ToString() + "?", temp);
            Phone_Number p = JsonConvert.DeserializeObject<Phone_Number>(Response);
            return p;
        }

        public Phone_Number GetClientPhoneNumber(int clientid, int phonenumberid)
        {
            string Response = GetAPIResponseTest("clients/" + clientid.ToString() + "/phone_numbers/" + phonenumberid.ToString());
            Phone_Number p = JsonConvert.DeserializeObject<Phone_Number>(Response);
            return p;
        }

        public List<Phone_Number> GetAllClientPhoneNumbers(int id)
        {
            string Response = GetAPIResponseTest("clients/" + id.ToString() + "/phone_numbers?per_page=1?page=1");
            List<Phone_Number> p = new List<Phone_Number>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("clients/" + id.ToString() + "/phone_numbers?" + "per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                page = JObject.Parse(Response);
                List<JToken> items = page["phone_numbers"].Children().ToList();
                foreach (JToken item in items)
                {
                    Phone_Number phone_number = JsonConvert.DeserializeObject<Phone_Number>(item.ToString());
                    p.Add(phone_number);
                }
            }
            return p;
        }

        //add one credit card at a time, address_id corresponds to billing address
        //TODO NOT FINISHED AND NOT TESTED
        public Credit_Card AddClientCreditCard(int clientid, int address_id, int phone_number_id, string number, int verification_code, int expiration_month, int expiration_year, string name, Dictionary<string, string> optional_values)
        {
            string temp = "{\"credit_cards\":[{";
            temp = temp + "\"address_id\":\"" + address_id.ToString() + "\",";
            temp = temp + "\"phone_number_id\":\"" + phone_number_id.ToString() + "\",";
            //temp = temp + "\"ip_address\":\"" + ip_address + "\",";
            temp = temp + "\"verification_code\":\"" + verification_code.ToString() + "\",";
            temp = temp + "\"number\":\"" + number + "\",";
            temp = temp + "\"expiration_month\":\"" + expiration_month.ToString() + "\",";
            temp = temp + "\"expiration_year\":\"" + expiration_year.ToString() + "\",";
            temp = temp + "\"name\":\"" + name + "\",";
            foreach (var v in optional_values)
            {
                temp = temp + "\"" + v.Key + "\":\"" + v.Value + "\",";
            }
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("clients/" + clientid.ToString() + "/credit_cards?", temp);
            Credit_Card c = JsonConvert.DeserializeObject<Credit_Card>(Response);
            return c;
        }

        //TODO NOT FINISHED AND NOT TESTED
        public List<Credit_Card> GetAllClientCreditCards(int id)
        {
            //string Response = GetAPIResponseTest("clients/" + id.ToString() + "/credit_cards?per_page=1?page=1");
            string Response = CreditCard_GetAPIResponseTest("clients/" + id.ToString() + "/credit_cards");
            List<Credit_Card> cc = new List<Credit_Card>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = CreditCard_GetAPIResponseTest("clients/" + id.ToString() + "/credit_cards?" + "per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                page = JObject.Parse(Response);
                List<JToken> items = page["credit_cards"].Children().ToList();
                foreach (JToken item in items)
                {
                    Credit_Card credit_card = JsonConvert.DeserializeObject<Credit_Card>(item.ToString());
                    cc.Add(credit_card);
                }
            }
            return cc;
        }

        //string[] order: ticket_group_id, quantity, price, only 
        //only one item allowed per order per TE rules
        public Order CreateBrokerageOrder(int buyer_id, int ticket_group_id, int quantity, decimal price, string invoice_number, decimal shipping, decimal service_fee, decimal tax, decimal additional_expense, string instructions, string po_number)
        {
            string temp = "{\"orders\":[{";
            temp = temp + "\"buyer_id\":\"" + buyer_id.ToString() + "\",";
            temp = temp + "\"items\":[";
            temp = temp + "{";
            temp = temp + "\"ticket_group_id\":\"" + ticket_group_id.ToString() + "\",";
            temp = temp + "\"quantity\":" + quantity.ToString() + ",";
            temp = temp + "\"price\":\"" + price.ToString() + "\"";
            temp = temp + "}";
            temp = temp + "],";
            if (invoice_number != "")
                temp = temp + "\"invoice_number\":\"" + invoice_number + "\",";
            if (shipping != 0)
                temp = temp + "\"shipping\":\"" + shipping.ToString() + "\",";
            if (service_fee != 0)
                temp = temp + "\"service_fee\":\"" + service_fee.ToString() + "\",";
            if (tax != 0)
                temp = temp + "\"tax\":\"" + tax.ToString() + "\",";
            if (additional_expense != 0)
                temp = temp + "\"additional_expense\":\"" + additional_expense.ToString() + "\",";
            if (instructions != "")
                temp = temp + "\"instructions\":\"" + instructions + "\",";
            if (po_number != "")
                temp = temp + "\"po_number\":\"" + po_number + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("orders?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            JObject OrderSet = JObject.Parse(Response);
            List<JToken> my_items = OrderSet["orders"].Children().ToList();
            List<Order> o = new List<Order>();
            foreach (JToken item in my_items)
            {
                Order order = JsonConvert.DeserializeObject<Order>(item.ToString());
                o.Add(order);
            }
            if (o.Count > 0)
                return o[0];
            else
                return null;
        }

        //Credit cards currently not allowed as payment_type option, 'offline' only
        //TODO add creditcards
        //string[] order is ticket_group_id, quantity, price
        //only one item allowed per order per TE rules
        public Order CreateCustomerOrder(int client_id, int seller_id, int ticket_group_id, int quantity, decimal price, int shipping_address_id, string po_number, string invoice_number, decimal shipping, decimal service_fee, decimal tax, decimal additional_expense, string instructions, int billing_address_id)
        {
            string temp = "{\"orders\":[{";
            temp = temp + "\"client_id\":" + client_id.ToString() + ",";
            temp = temp + "\"seller_id\":" + seller_id.ToString() + ",";
            temp = temp + "\"items\":[";
            temp = temp + "{";
            temp = temp + "\"ticket_group_id\":\"" + ticket_group_id.ToString() + "\",";
            temp = temp + "\"quantity\":" + quantity.ToString() + ",";
            temp = temp + "\"price\":\"" + price.ToString() + "\"";
            temp = temp + "}";
            temp = temp + "],";
            temp = temp + "\"payments\":[{\"type\":\"" + "offline" + "\"}],"; //change later for credit cards
            if (invoice_number != "" && invoice_number != null)
                temp = temp + "\"invoice_number\":\"" + invoice_number + "\",";
            if (shipping != 0)
                temp = temp + "\"shipping\":\"" + shipping.ToString() + "\",";
            if (service_fee != 0)
                temp = temp + "\"service_fee\":\"" + service_fee.ToString() + "\",";
            if (tax != 0)
                temp = temp + "\"tax\":\"" + tax.ToString() + "\",";
            if (additional_expense != 0)
                temp = temp + "\"additional_expense\":\"" + additional_expense.ToString() + "\",";
            if (instructions != "" && instructions != null)
                temp = temp + "\"instructions\":\"" + instructions + "\",";
            if (po_number != "" && po_number != null)
                temp = temp + "\"po_number\":\"" + po_number + "\",";
            if (shipping_address_id != 0)
                temp = temp + "\"shipping_address_id\":" + shipping_address_id.ToString() + ",";
            if (billing_address_id != 0)
                temp = temp + "\"billing_address_id\":" + billing_address_id.ToString() + ",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("orders?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            JObject OrderSet = JObject.Parse(Response);
            List<JToken> my_items = OrderSet["orders"].Children().ToList();
            List<Order> o = new List<Order>();
            foreach (JToken item in my_items)
            {
                Order order = JsonConvert.DeserializeObject<Order>(item.ToString());
                o.Add(order);
            }
            if (o.Count > 0)
                return o[0];
            else
                return null;
        }

        public Order UpdateCustomerOrder(int order_id, string po_number, string invoice_number, string instructions, int billing_address_id, int shipping_address_id)
        {
            string temp = "{";
            if(po_number != "")
                temp = temp + "\"po_number\":\"" + po_number + "\",";
            if(invoice_number != "")
                temp = temp + "\"invoice_number\":\"" + invoice_number + "\",";
            if(instructions != "")
                temp = temp + "\"instructions\":\"" + instructions + "\",";
            if(billing_address_id != 0)
                temp = temp + "\"billing_address_id\":" + billing_address_id.ToString() + ",";
            if(shipping_address_id != 0)
                temp = temp + "\"shipping_address_id\":" + shipping_address_id.ToString() + ",";
            if(temp != "{")
                temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("orders/" + order_id.ToString() + "?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            Order order = JsonConvert.DeserializeObject<Order>(Response);
            return order;
        }

        public Order UpdateBrokerageOrder(int order_id, string po_number, string invoice_number, string instructions)
        {
            string temp = "{";
            if(po_number != "")
                temp = temp + "\"po_number\":\"" + po_number + "\",";
            if(invoice_number != "")
                temp = temp + "\"invoice_number\":\"" + invoice_number + "\",";
            if(instructions != "")
                temp = temp + "\"instructions\":\"" + instructions + "\",";
            if(temp != "{")
                temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("orders/" + order_id.ToString() + "?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            Order order = JsonConvert.DeserializeObject<Order>(Response);
            return order;
        }

        public void AcceptOrder(int order_id, int reviewer_id)
        {
            string temp = "{\"reviewer_id\":" + reviewer_id.ToString() + "}";
            string Response = PostAPIResponseTest("orders/" + order_id.ToString() + "/accept",temp);
        }

        //per TE rules, only one order item at a time
        public Order CreateFullfillmentOrder(int buyer_id, int ticket_group_id, int quantity, decimal price, string invoice_number, decimal shipping, decimal service_fee, decimal tax, decimal additional_expense, string instructions, string po_number)
        {
            string temp = "{\"orders\":[{";
            temp = temp + "\"buyer_id\":\"" + buyer_id.ToString() + "\",";
            temp = temp + "\"items\":[";
                temp = temp + "{";
                temp = temp + "\"ticket_group_id\":\"" + ticket_group_id.ToString() + "\",";
                temp = temp + "\"quantity\":" + quantity.ToString() + "\",";
                temp = temp + "\"price\":\"" + price.ToString() + "\"";
                temp = temp + "}";
            temp = temp + "],";
            if(invoice_number != "")
                temp = temp + "\"invoice_number\":\"" + invoice_number + "\",";
            if(shipping != 0)
                temp = temp + "\"shipping\":\"" + shipping.ToString() + "\",";
            if(service_fee != 0)
                temp = temp + "\"service_fee\":\"" + service_fee.ToString() + "\",";
            if(tax != 0)
                temp = temp + "\"tax\":\"" + tax.ToString() + "\",";
            if(additional_expense != 0)
                temp = temp + "\"additional_expense\":\"" + additional_expense.ToString() + "\",";
            if(instructions != "")
                temp = temp + "\"instructions\":\"" + instructions + "\",";
            if(po_number != "")
                temp = temp + "\"po_number\":\"" + po_number + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("orders/fulfillments?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            JObject OrderSet = JObject.Parse(Response);
            List<JToken> my_items = OrderSet["orders"].Children().ToList();
            List<Order> o = new List<Order>();
            foreach (JToken item in my_items)
            {
                Order order = JsonConvert.DeserializeObject<Order>(item.ToString());
                o.Add(order);
            }
            if (o.Count > 0)
                return o[0];
            else
                return null;
        }

        public void RejectOrder(int order_id, int reviewer_id, bool not_available, bool priced_incorrectly, bool duplicate, bool fraudulent)
        {
            string temp = "{";
            if(not_available == true)
                temp = temp + "\"rejection_reason\":\"Tickets No Longer Available\",\"reviewer_id\":" + reviewer_id.ToString() + "}";
             if(priced_incorrectly == true)
                temp = temp + "\"rejection_reason\":\"Tickets Priced Incorrectly\",\"reviewer_id\":" + reviewer_id.ToString() + "}";
             if(duplicate == true)
                temp = temp + "\"rejection_reason\":\"Duplicate Order\",\"reviewer_id\":" + reviewer_id.ToString() + "}";
             if(fraudulent == true)
                temp = temp + "\"rejection_reason\":\"Fraudulent Order\",\"reviewer_id\":" + reviewer_id.ToString() + "}";
             string Response = PostAPIResponseTest("orders/" + order_id.ToString() + "/reject",temp);
        }

        public void CompleteOrder(int order_id)
        {
            string temp = "{}";
            string Response = PostAPIResponseTest("orders/" + order_id.ToString() + "/complete",temp);
        }

        public Order GetOrder(int id)
        {
            string Response = GetAPIResponseTest("orders/" + id.ToString());
            //string Response = GetAPIResponse("orders/" + id.ToString());
            Response = Response.Replace("\"event\":", "event_:");
            Order order = new Order();
            order = JsonConvert.DeserializeObject<Order>(Response);
            return order;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> o = new List<Order>();
            string Response = GetAPIResponseTest("orders?page=1?per_page=1");
            //string Response = GetAPIResponse("orders?page=1?per_page=1");
            Response = Response.Replace("\"event\":", "event_:");
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("orders?per_page=" + per_page.ToString() + "?" + "page=" + (i+1).ToString());
                //Response = GetAPIResponse("orders?per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                Response = Response.Replace("\"event\":", "event_:");
                page = JObject.Parse(Response);
                List<JToken> items = page["orders"].Children().ToList();
                foreach (JToken item in items)
                {
                    Order order = JsonConvert.DeserializeObject<Order>(item.ToString());
                    o.Add(order);
                }
            }
            return o;
        }

        //pdf is base64-encoded, items is a list of item id numbers to include in shipment
        public Shipment AddOrderShipment(int order_id, bool fedex, bool ups, bool courier, string pdf, List<int> items, string tracking_number, string service_type)
        {
            string my_pdf = pdf;
            string temp = "{\"shipments\":[{";
            temp = temp + "\"order_id\":\"" + order_id.ToString() + "\",";
            if(fedex == true)
                temp = temp + "\"type\":\"" + "FedEx" + "\",";
            if (ups == true)
                temp = temp + "\"type\":\"" + "UPS" + "\",";
            if (courier == true)
                temp = temp + "\"type\":\"" + "Courier" + "\",";
            temp = temp + "\"airbill\":\"" + my_pdf + "\",";
            temp = temp + "\"items\":[";
            foreach (int i in items)
            {
                temp = temp + "{\"id\":\"" + i.ToString() + "\"},";
            }
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "],";

            if (tracking_number != "" && tracking_number != null)
                temp = temp + "\"tracking_number\":\"" + tracking_number + "\",";
            if (service_type != "" && service_type != null)
                temp = temp + "\"service_type\":\"" + service_type + "\",";
            temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}]}";
            string Response = PostAPIResponseTest("shipments?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            JObject OrderSet = JObject.Parse(Response);
            List<JToken> my_items = OrderSet["shipments"].Children().ToList();
            List<Shipment> s = new List<Shipment>();
            foreach (JToken item in my_items)
            {
                Shipment shipment = JsonConvert.DeserializeObject<Shipment>(item.ToString());
                s.Add(shipment);
            }
            if (s.Count > 0)
                return s[0];
            else
                return null;
        }

        //pdf is base64-encoded, items is a list of item id numbers to include in shipment
        public Shipment UpdateOrderShipment(int shipment_id, bool update_shipping_type, bool fedex, bool ups, bool courier, string pdf, string tracking_number, string service_type)
        {
            string my_pdf = pdf;
            string temp = "{";
            if (update_shipping_type == true)
            {
                if (fedex == true)
                    temp = temp + "\"type\":\"" + "FedEx" + "\",";
                if (ups == true)
                    temp = temp + "\"type\":\"" + "UPS" + "\",";
                if (courier == true)
                    temp = temp + "\"type\":\"" + "Courier" + "\",";
            }
            if (my_pdf != "")
                temp = temp + "\"airbill\":\"" + my_pdf + "\",";
            if (tracking_number != "" && tracking_number != null)
                temp = temp + "\"tracking_number\":\"" + tracking_number + "\",";
            if (service_type != "" && service_type != null)
                temp = temp + "\"service_type\":\"" + service_type + "\",";
            if(temp != "{")
                temp = temp.Substring(0, temp.Length - 1);
            temp = temp + "}";
            string Response = PutAPIResponseTest("shipments/" + shipment_id.ToString() + "?", temp);
            Response = Response.Replace("\"event\":", "event_:");
            Shipment shipment = JsonConvert.DeserializeObject<Shipment>(Response);
            return shipment;
        }

        public Shipment GetOrderShipment(int shipment_id)
        {
            string Response = GetAPIResponseTest("shipments/" + shipment_id.ToString() + "?");
            Response = Response.Replace("\"event\":", "event_:");
            Shipment s = JsonConvert.DeserializeObject<Shipment>(Response);
            return s;
        }

        public List<Shipment> GetAllOrderShipments()
        {
            //string Response = GetAPIResponseTest("shipments?per_page=1?page=1");
            string Response = GetAPIResponse("shipments?per_page=1?page=1");
            Response = Response.Replace("\"event\":", "event_:");
            List<Shipment> s = new List<Shipment>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                //Response = GetAPIResponseTest("shipments?per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                Response = GetAPIResponse("shipments?per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                Response = Response.Replace("\"event\":", "event_:");
                page = JObject.Parse(Response);
                List<JToken> items = page["shipments"].Children().ToList();
                foreach (JToken item in items)
                {
                    Shipment shipment = JsonConvert.DeserializeObject<Shipment>(item.ToString());
                    s.Add(shipment);
                }
            }
            return s;
        }

        public List<Quote> GetAllQuotes()
        {
            string Response = GetAPIResponseTest("quotes?per_page=1?page=1");
            Response = Response.Replace("\"event\":", "event_:");
            List<Quote> q = new List<Quote>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("quotes?per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString());
                Response = Response.Replace("\"event\":", "\"event_\":");
                page = JObject.Parse(Response);
                List<JToken> items = page["quotes"].Children().ToList();
                foreach (JToken item in items)
                {
                    Quote quote = JsonConvert.DeserializeObject<Quote>(item.ToString());
                    q.Add(quote);
                }
            }
            return q;
        }

        public Quote GetQuote(int quote_id)
        {
            string Response = GetAPIResponseTest("quotes/" + quote_id.ToString());
            Response = Response.Replace("\"event\":", "\"event_\":");
            Quote q = JsonConvert.DeserializeObject<Quote>(Response);
            return q;
        }

        public List<Quote> SearchQuotes(string s)
        {
            string Response = GetAPIResponseTest("quotes/search?per_page=1?page=1?q=\"" + s + "\"");
            List<Quote> q = new List<Quote>();
            JObject page = JObject.Parse(Response);
            JToken results = page["total_entries"];
            int total = JsonConvert.DeserializeObject<int>(results.ToString());
            double mytotal = Convert.ToDouble(total);
            double per_page = 100;
            double pages = (total / per_page);

            for (double i = 0; i < pages; i++)
            {
                Response = GetAPIResponseTest("quotes/search?per_page=" + per_page.ToString() + "?" + "page=" + (i + 1).ToString()) + "?q=\"" + s + "\"";
                Response = Response.Replace("\"event\":", "\"event_\":");
                page = JObject.Parse(Response);
                List<JToken> items = page["quotes"].Children().ToList();
                foreach (JToken item in items)
                {
                    Quote quote = JsonConvert.DeserializeObject<Quote>(item.ToString());
                    q.Add(quote);
                }
            }
            return q;
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

        public List<T> GetPostResponse<T>(string response)
        {
            string typeName = typeof(T).Name.ToLower();
            if (typeName == "category") typeName = "categorie";
            string Response = response;
            JObject resultSet = JObject.Parse(Response);
            JToken results = resultSet["1"];
            List<JToken> items = resultSet[typeName + "s"].Children().ToList();
            List<T> rList = new List<T>();
            foreach (JToken item in items)
            {
                rList.Add(JsonConvert.DeserializeObject<T>(item.ToString()));
            }
            return rList;
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

        public string PutAPIResponseTest(string url, string body)
        {
            string api = "api.sandbox.ticketevolution.com/" + url + body;
            string token = "5c1d05bae1ddedfbd065939c9525dc24";
            string secret = "kzjVrAGO4zPBCYIUXqDtzz6UVShL+nDlv5nnpY+8";
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bRequest = encoding.GetBytes("PUT " + api);
            byte[] bSecretkey = encoding.GetBytes(secret);

            HMACSHA256 myhmacsha256 = new HMACSHA256(bSecretkey);
            myhmacsha256.ComputeHash(bRequest);
            string X_Signature = Convert.ToBase64String(myhmacsha256.Hash);

            byte[] send_body = encoding.GetBytes(body);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + "api.sandbox.ticketevolution.com/" + url);
            request.Method = "PUT";
            request.ContentLength = send_body.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/vnd.ticketevolution.api+json; version=8";
            request.Headers.Add("X-Signature", X_Signature);
            request.Headers.Add("X-Token", token);

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(send_body, 0, send_body.Length);
            dataStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader loResponseStream = new
                StreamReader(response.GetResponseStream());

            string Response = loResponseStream.ReadToEnd();
            response.Close();
            return Response;
        }

        public string GetAPIResponseTest(string urlParams)
        {
            string api = "api.sandbox.ticketevolution.com/" + urlParams;
            string token = "5c1d05bae1ddedfbd065939c9525dc24";
            string secret = "kzjVrAGO4zPBCYIUXqDtzz6UVShL+nDlv5nnpY+8";
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

        public string CreditCard_GetAPIResponseTest(string urlParams)
        {
            string api = "api.sandbox.ticketevolution.com/" + urlParams;
            string token = "5c1d05bae1ddedfbd065939c9525dc24";
            string secret = "kzjVrAGO4zPBCYIUXqDtzz6UVShL+nDlv5nnpY+8";
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bRequest = encoding.GetBytes("GET " + api);
            byte[] bSecretkey = encoding.GetBytes(secret);

            HMACSHA256 myhmacsha256 = new HMACSHA256(bSecretkey);
            myhmacsha256.ComputeHash(bRequest);
            string X_Signature = Convert.ToBase64String(myhmacsha256.Hash);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + api);
            request.Method = "GET";
            //request.ContentType = "application/json";
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

        public string PostAPIResponseTest(string url, string body)
        {
            string api = "api.sandbox.ticketevolution.com/" + url + body;
            //string token = "5963063fd7aa7f403a63167f212ad205";
            //string secret = "QWgOU1z0FNlnop5HBZCKKk/nUZ3EuLByFWlk71OE";
            string token = "5c1d05bae1ddedfbd065939c9525dc24";
            string secret = "kzjVrAGO4zPBCYIUXqDtzz6UVShL+nDlv5nnpY+8";
            //string token = "de754476c55950df4f894611dddf7c77";
            //string secret = "e/sX7DLBWVvWJg9VHAbzwemeGcR9BYeio5EC0Fw8";
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bRequest = encoding.GetBytes("POST " + api);
            byte[] bSecretkey = encoding.GetBytes(secret);

            HMACSHA256 myhmacsha256 = new HMACSHA256(bSecretkey);
            myhmacsha256.ComputeHash(bRequest);
            string X_Signature = Convert.ToBase64String(myhmacsha256.Hash);

            byte[] send_body = encoding.GetBytes(body);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://" + "api.sandbox.ticketevolution.com/" + url);
            request.Method = "POST";
            request.ContentLength = send_body.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/vnd.ticketevolution.api+json; version=8";
            request.Headers.Add("X-Signature", X_Signature);
            request.Headers.Add("X-Token", token);

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(send_body, 0, send_body.Length);
            dataStream.Close();

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
        public int id { get; set; }
        public string name { get; set; }
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

    //add more parameters?
    public class Client
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<Address> addresses { get; set; }
        public List<Email_Address> email_addresses { get; set; }
        public List<Phone_Number> phone_numbers { get; set; }
    }

    public class Email_Address
    {
        public int id { get; set; }
        public string address { get; set; }
    }

    public class Phone_Number
    {
        public int id { get; set; }
        public string country_code { get; set; }
        public string extension { get; set; }
        public string number { get; set; }
    }

    public class Credit_Card
    {
        public int id { get; set; }
        public int credit_card_id { get; set; }
        public Association association { get; set; }
        public int expiration_month { get; set; }
        public string name { get; set; }
        public int expiration_year { get; set; }
        public int last_digits { get; set; }
        public string ip_address { get; set; }
        public int verification_code { get; set; }
        public int number { get; set; }
        public Address address { get; set; }
        public Phone_Number phone_number { get; set; }
    }

    public class Association
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public TicketGroup ticket_group { get; set; }
        public int index { get; set; }
    }

    public class Payment
    {
        public int id { get; set; }
        public string type { get; set; }
        public Credit_Card credit_card { get; set; }
        public string avs_response { get; set; }
        public int expiration_year { get; set; }
        public Phone_Number phone_number { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
        public decimal amount { get; set; }
        public string cvv_response { get; set; }
    }

    public class Order
    {
        public int id { get; set; }
        public List<Item> items { get; set; }
        public decimal tax { get; set; }
        public string placer { get; set; }
        public Office seller { get; set; }
        public DateTime? hold_expires_at { get; set; }
        public DateTime? created_at { get; set; }
        public decimal refunded { get; set; }
        public DateTime? hold_placed_at { get; set; }
        public Office buyer { get; set; }
        public DateTime? updated_at { get; set; }
        public decimal total { get; set; }
        public decimal additional_expense { get; set; }
        public List<Payment> payments { get; set; }
        public Address shipping_address { get; set; }
        public decimal shipping { get; set; }
        public string invoice_number { get; set; }
        public Client client { get; set; }
        public Address billing_address { get; set; }
        public decimal subtotal { get; set; }
        public string po_number { get; set; }
        public string instructions { get; set; }
        public decimal service_fee { get; set; }
        public decimal balance { get; set; }
        public string state { get; set; }
        public List<int> child_orders { get; set; }
    }

    public class Shipment
    {
        public int id { get; set; }
        public List<Item> items { get; set; }
        public Order order { get; set; }
        public string state { get; set; }
        public string type { get; set; }
        public string service_type { get; set; }
        public string airbill { get; set; } //url of uploaded pdf airbill
        public string tracking_number { get; set; }
    }

    public class Recipient
    {
        public Email_Address email { get; set; }
        public string name { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Quote
    {
        public int id { get; set; }
        public List<Item> items { get; set; }
        public Recipient recipient { get; set; }
        public Event event_ { get; set; }
        public User user { get; set; }
        public decimal _score { get; set; }
        public string _type { get; set; }
    }

    public class Account
    {
        public int id { get; set;}
        public string currency { get; set; }
        public decimal balance { get; set; }
        public Client client { get; set; }
    }

}
