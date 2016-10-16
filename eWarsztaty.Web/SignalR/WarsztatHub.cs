using eWarsztaty.Domain;
using eWarsztaty.Web.Helpers;
using eWarsztaty.Web.Infrastructure;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace eWarsztaty.Web.SignalR
{

    public class WarsztatHub : Hub
    {
        public async Task JoinGroup(int warsztatId)
        {
            using (var _db = new eWarsztatyContext())
            {
                await Groups.Add(Context.ConnectionId, warsztatId.ToString());
                var warsztat = _db.Warsztaty.Include("Prowadzacy").FirstOrDefault(x => x.WarsztatId == warsztatId);
                string prowadzacy = warsztat.Prowadzacy.Login;
                int id = DBHelpers.GetUserId(Context.User.Identity.Name);
                var udzial = _db.UdzialyWWarsztacie.FirstOrDefault(x => x.UzytkownikId == id && x.WarsztatId == warsztatId);
                udzial.Aktywny = true;

                if (id != warsztat.ProwadzacyId)
                    Clients.Client(warsztat.ProwadzacyConnectionId).RefreshUserGrid(Context.User.Identity.Name + " dołączył do warsztatu");
                else
                    warsztat.ProwadzacyConnectionId = Context.ConnectionId;
                _db.SaveChanges();
            }
        }

        public async Task ClientJoin(string login, string password)
        {
            using (var _db = new eWarsztatyContext())
            {
                if (!Membership.ValidateUser(login, password))
                {
                    Clients.Client(Context.ConnectionId).AgentAuthorisation("Niepoprawny login lub hasło", false);
                }
                else
                {
                    Clients.Client(Context.ConnectionId).AgentAuthorisation("", true);
                    if (_db.UdzialyWWarsztacie.Include("Uzytkownik").Any(x => x.Uzytkownik.Login == login && x.AktywnyAgent == true))
                    {
                        Clients.Client(Context.ConnectionId).AgentNoParticipate("Użytkownik: " + login + " ma już uruchomionego agenta");
                    }
                    var udzial = _db.UdzialyWWarsztacie.Include("Uzytkownik").Include("Warsztat.Prowadzacy").FirstOrDefault(x => x.Uzytkownik.Login == login && x.Aktywny == true);
                    if (udzial == null)
                        Clients.Client(Context.ConnectionId).AgentNoParticipate("Użytkownik: " + login + " nie bierze udziału w żadnym warsztacie");
                    else
                    {
                        int warsztatId = udzial.WarsztatId;
                        //var udzial = _db.UdzialyWWarsztacie.Include("Uzytkownik").FirstOrDefault(x => x.Aktywny == true && x.Uzytkownik.Login == login && x.WarsztatId == warsztatId);
                        udzial.AktywnyAgent = true;
                        udzial.AgentConnectionId = Context.ConnectionId;
                        _db.SaveChanges();
                        Clients.Client(GetPorwadzacyConnectionIdByWarsztatId(warsztatId)).RefreshUserGrid("");

                        Clients.User(login).SucceedAgentConnection("Agent uruchomiony poprawnie");

                        await Groups.Add(Context.ConnectionId, warsztatId.ToString());
                        Clients.Client(Context.ConnectionId).AgentMakeConnection(warsztatId.ToString());
                    }
                }
            }

        }

        public override Task OnDisconnected(bool stopCalled = true)
        {
            using (var _db = new eWarsztatyContext())
            {
                //Groups.Remove(Context.ConnectionId, UserHandler.groupName);
                if (String.IsNullOrEmpty(Context.User.Identity.Name)) //rozlaczyl się agent
                {
                    var udzial = _db.UdzialyWWarsztacie.Include("Uzytkownik").Include("Warsztat.Prowadzacy").FirstOrDefault(x => x.AgentConnectionId == Context.ConnectionId);
                    udzial.AgentConnectionId = "";
                    udzial.AktywnyAgent = false;
                    _db.SaveChanges();
                    Clients.Client(udzial.Warsztat.ProwadzacyConnectionId).RefreshUserGrid("");

                    Clients.User(udzial.Uzytkownik.Login).DisconnectAgent("Agent rozłączony, włącz go ponownie");
                }
                else
                {
                    int id = DBHelpers.GetUserId(Context.User.Identity.Name);

                    string warsztatIdHeader = Context.QueryString["warsztatId"];
                    int warsztatId = Convert.ToInt32(warsztatIdHeader);
                    var udzial = _db.UdzialyWWarsztacie.Include("Warsztat.Prowadzacy").FirstOrDefault(x => x.UzytkownikId == id && x.WarsztatId == warsztatId);
                    var prowadzacy = udzial.Warsztat.Prowadzacy.Login;
                    if (!String.IsNullOrEmpty(udzial.Warsztat.ProwadzacyConnectionId))
                        Clients.Client(udzial.Warsztat.ProwadzacyConnectionId).RefreshUserGrid("");

                    if (prowadzacy == Context.User.Identity.Name)
                        udzial.Warsztat.ProwadzacyConnectionId = "";
                    udzial.Aktywny = false;
                    _db.SaveChanges();

                    //UserHandler.ConnectedIds.Remove(Context.ConnectionId);

                    //jezeli prowadzacy opusci warsztat to alert
                    if (DBHelpers.GetUserId(prowadzacy) == id)
                    {
                        Clients.Group(warsztatId.ToString()).ProwadzacyQuitAlert();
                    }
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public void CloseWarsztat(int warsztatId)
        {
            using (var _db = new eWarsztatyContext())
            {
                //usuwamy udzial z warsztatu
                var udzialy = _db.UdzialyWWarsztacie.Where(x => x.Aktywny == true && x.WarsztatId == warsztatId);
                foreach (var udzial in udzialy)
                    udzial.Aktywny = false;

                var warsztat = _db.Warsztaty.FirstOrDefault(x => x.WarsztatId == warsztatId);
                warsztat.StatusWarsztatu = (int)eWarsztatyEnums.StatusWarsztatu.Zakonczony;
                _db.SaveChanges();

                //dla wszystkich czlonkow grupy wywolujemy metode z relocatem
                Clients.Group(warsztatId.ToString()).Relocate();

                //Samo sie usunie jak wszysyc zrobia relocate
                ////usuwamy wszystkich z grupy signalR
                //foreach (var context in UserHandler.ConnectedIds)
                //    Groups.Remove(context, warsztatId.ToString());

                //UserHandler.ConnectedIds.Clear();

            }
        }

        public void TakeUserScreenShot(string login, int warsztatId)
        {
            using (var _db = new eWarsztatyContext())
            {
                string agentContext = _db.UdzialyWWarsztacie.Include("Uzytkownik").FirstOrDefault(x => x.Uzytkownik.Login == login && x.AktywnyAgent == true).AgentConnectionId;
                //string agentConnectionString = UserHandler.UsersDict.FirstOrDefault(x => x.Key == login).Value.Item2;
                Clients.Client(agentContext).AgentMakeScreenShot(login, warsztatId);
            }
        }

        public async Task ShowScreenImage(byte[] imgBytes, string login, int warsztatId)
        {
            if (imgBytes != null)
            {
                Image img = byteArrayToImage(imgBytes);
                string image64 = Convert.ToBase64String(imgBytes);
                await Clients.Client(GetPorwadzacyConnectionIdByWarsztatId(warsztatId)).ShowUserScreenImage(image64, login);
            }
        }

        public void LoadPresentation(int plikId, int warsztatId)
        {
            using (var _db = new eWarsztatyContext())
            {
                var plik = _db.Pliki.Include("Warsztat").FirstOrDefault(x => x.PlikId == plikId);
                if (plik != null)
                {
                    int id = DBHelpers.GetUserId(Context.User.Identity.Name);
                    if (id == plik.Warsztat.ProwadzacyId)
                    {
                        Clients.Group(warsztatId.ToString()).LoadPresentationToClients(plikId);
                    }
                }

            }
        }

        public void ChangePresentationPage(int operation, int warsztatId)
        {
            if (operation == (int)eWarsztatyEnums.PrezentacjaZmianaStrony.Nastepna)
                Clients.Group(warsztatId.ToString()).ChangePresentationPageFromHub((int)eWarsztatyEnums.PrezentacjaZmianaStrony.Nastepna);
            else
                Clients.Group(warsztatId.ToString()).ChangePresentationPageFromHub((int)eWarsztatyEnums.PrezentacjaZmianaStrony.Poprzednia);

        }

        public void DownloadExercise(int plikId, string login)
        {
            using (var _db = new eWarsztatyContext())
            {
                var exercise = _db.Pliki.Include("Warsztat").FirstOrDefault(x => x.PlikId == plikId);
                if (exercise == null)
                    Clients.User(login).ShowDownloadExerciseWarning("Błąd, nie odnalezionio pliku");
                else if (exercise.Warsztat.ProwadzacyId == DBHelpers.GetUserId(login))
                {
                    Clients.Client(GetPorwadzacyConnectionIdByWarsztatId(exercise.WarsztatId)).ShowDownloadExerciseWarning("Prowadzacy nie musi pobierac zadania");
                }
                else
                {
                    var agent = _db.UdzialyWWarsztacie.Include("Uzytkownik").FirstOrDefault(x => x.Uzytkownik.Login == login && x.AktywnyAgent == true);
                    //var agentContext = UserHandler.UsersDict.FirstOrDefault(x => x.Key == login).Value.Item2;
                    if (agent != null && !String.IsNullOrEmpty(agent.AgentConnectionId))
                    {
                        Clients.Client(agent.AgentConnectionId).AgentDownloadExercise(exercise.File, exercise.Rozszerzenie, exercise.Nazwa, exercise.Warsztat.Temat);
                    }
                    else
                        Clients.User(login).ShowDownloadExerciseWarning("Aby pobrać zadanie, należy uruchomić agenta");
                }
            }
        }

        public void RefreshPlikiGrids(int warsztatId)
        {

            Clients.Group(warsztatId.ToString()).RefreshPliki();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private static string GetProwadzacyByWarsztatId(int warsztatId)
        {
            using (var _db = new eWarsztatyContext())
            {

                return _db.Warsztaty.Include("Prowadzacy").FirstOrDefault(x => x.WarsztatId == warsztatId).Prowadzacy.Login;
            }
        }

        private static string GetPorwadzacyConnectionIdByWarsztatId(int? warsztatId)
        {

            using (var _db = new eWarsztatyContext())
            {

                return _db.Warsztaty.Include("Prowadzacy").FirstOrDefault(x => x.WarsztatId == warsztatId).ProwadzacyConnectionId;
            }
        }
    }
}