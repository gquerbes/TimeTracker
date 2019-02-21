using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using TimeTracker.Services;

namespace TimeTracker.Database
{
    /// <summary>
    /// Loads all available tickets fro JIRA into the app
    /// </summary>
    public class TicketLoader
    {
        static HttpClient client = new HttpClient();
        public static void LoadData()
        {
            //var rawData = RepliConnect.GetTickets();

            



            //foreach (var ticket in X)
            //{
            //    if (!App.Database.Query<Ticket>($"id = {ticket.id}").Any())
            //    {
            //        //set summary directly on ticket object from fields object
            //        ticket.Summary = ticket.fields.summary;
            //        //set RepliconID directly on ticket object from fields object
            //        ticket.repliconID = ticket.fields.customfield_10571;
            //        //Save ticket 
            //        App.Database.SaveItem(ticket);
            //    }
            //}
        }

    
    }

     
    }

    public class converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }


