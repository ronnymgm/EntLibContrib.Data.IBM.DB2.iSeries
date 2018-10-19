//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    [TestClass]
    public class Db2ParameterFixture
    {
        private readonly string BIGSTRING = @"Loremipsumdolorsitamet,consecteturadipiscingelit.Aeneanatempusneque,acondimentumleo.Loremipsumdolorsitamet,consecteturadipiscingelit.Curabiturconsecteturmaximusjustovitaepellentesque.Nullamcongueportavarius.Quisqueatenimturpis.Maecenastempornisivelantemollisullamcorpervelvitaerisus.Sedacpurusetnullaultriciesfinibusaceuante.Donecdignissimnullanunc.Maecenastincidunt,duinecdignissimconsectetur,dolorodiosodaleslacus,placerateuismodestexinmauris.Sedvitaefermentumorci.Crasiaculismaurissedloremcondimentumsuscipitquisutante.Orcivariusnatoquepenatibusetmagnisdisparturientmontes,nasceturridiculusmus.Duiscondimentumdictumsapienegetdictum.Nullalaoreetdiamacorcimattisornare.Phasellustristiquepulvinarultricies.Duissitametelementumturpis,ethendreritorci.Curabiturpellentesqueerategetipsumtemporfermentum.Donecmaximusullamcorpercommodo.Duisegetdolorscelerisque,rhoncusmetusvitae,placeratnunc.Fuscefeliseros,pretiumidfinibussitamet,suscipitnonmetus.Quisqueinterduminterdumligulasedultrices.Proinatfinibusjusto.Donecconguetempusultrices.Maecenasconsequatbibendummagna.Praesentaliquet,ipsumincursusluctus,purusesttristiquenisl,necfacilisisnislaugueetneque.Fuscepellentesquedignissimlacus,eueuismodmetusconvallisnon.Maecenaslobortistinciduntpulvinar.Maurisrhoncusegestasscelerisque.Aliquamsitametnisiatmetusaliquamvenenatisatsitametmi.Namaccumsanenimeuloremaliquetauctor.Namlaciniaullamcorpervehicula.Mauriseuismodmetusmetus.Maecenasfeugiatauguequisipsumfacilisisrutrum.Quisqueviverrasederatsedsollicitudin.Donectinciduntmattisest,sedegestasantelobortisa.Integersodalesquamsedtinciduntelementum.Utaliquetfeugiateros,velsodalesestdignissimsed.Sedconvallisvulputatepulvinar.Nullacongueeliteujustotinciduntpellentesquequissedfelis.Aliquameratvolutpat.Utvulputatemassaatlobortisvarius.Quisquemattisnibhsitameterospharetratempus.Ineleifendconvallisaugue,nonfacilisisduiullamcorperet.Pellentesquesitameterosmalesuada,mollisesteget,aliquamneque.Namintinciduntest,etfacilisisnibh.Vestibulumnonduidictum,convalliseratsitamet,bibendumjusto.Donectempor,nequeegetullamcorperegestas,urnalacusporttitordui,necfinibussapienmassaquiselit.Ininterdumnislatodioullamcorper,quispretiumlectuslacinia.Nammaximusvenenatisjustosempercursus.Doneciaculis,sapiensitametcondimentumcommodo,odioexgravidamassa,etvulputatelectuslacusquisaugue.Vestibulumviverrahendreritodioacvulputate.Maurisvelblanditquam.Aeneanviverra,quamsedimperdiettincidunt,mijustofringillaante,congueelementumleoenimegetmassa.Morbivelcommodomauris.Inhachabitasseplateadictumst.Proinquisipsummollis,semperturpisid,bibendumnisi.Phasellusinexmassa.Maurisvulputatesemuterosscelerisque,vitaepellentesqueduivestibulum.Namnislelit,commodoetesteu,hendreritbibendumodio.Nampulvinartortoratrisusmolestiedapibus.Pellentesquedictumnontelluseufaucibus.Proinatmattisrisus.Praesentbibendumsuscipitipsum,indapibusmetuslaoreetvarius.Proinconvallisquislectusabibendum.Donecsapiennisl,pulvinarquisnisia,elementumpulvinaripsum.Pellentesqueconguemiquam,idtempusduibibendumat.Praesentnecdiamdiam.Aeneanscelerisque,nislquisscelerisqueultricies,nislturpisconguemetus,euismodconvallisjustomaurisquiselit.Etiamquislaciniapurus.Aliquamornareelitsitametantelobortisiaculis.Inatestpurus.Maecenasorcilibero,auctoractinciduntet,aliquetatnisi.Curabiturrutrumlobortisodioportavarius.Duisaccumsanatipsumvitaeegestas.Pellentesquemalesuadacursusaliquam.Craseleifendsemperestnonsodales.Curabitureujustonulla.Vivamusvulputatenisleupurusornaresemper.Nuncfringillaeratvitaeipsumsodales,eugravidaloremefficitur.Namposuere,arcuvitaeornarefinibus,nullaeratdictumarcu,atfeugiaturnaodioetorci.Fusceutvelitquisipsumporttitordapibus.Vivamuscondimentumtemporodioeugravida.Integermassasapien,accumsanegetexeu,tinciduntrhoncusnunc.Sedetpurusidduidapibusgravida.Utfinibusfelisante,necpretiumrisusdignissimut.Nullametmattisarcu.Vestibulumportaquamsitametauctorsuscipit.Curabiturinfaucibusnibh,sedfringillasapien.VestibulumanteipsumprimisinfaucibusorciluctusetultricesposuerecubiliaCurae;Vivamuslaoreetleovehiculatristiquetincidunt.Praesentauctornuncnulla,sitamettinciduntnullamollisquis.Infeugiat,erosegetsempermolestie,nullaarcusodalesmagna,adictumsapiennequeeuorci.Utbibendumnullaacligulacondimentum,nechendreritexvarius.Doneciaculisloremquistelluselementum,nonpharetraduipharetra.Utegetfermentumaugue.Vivamusegestastortoripsum,infacilisisfelisgravidaat.Integerarcupurus,dictumapurusnec,lobortispretiumnunc.Insagittisduisedturpissollicitudin,idcondimentumexpulvinar.Integerodiolibero,mattisvelnibhac,porttitordapibussapien.Maurisatvenenatiseros,utgravidaurna.Praesentmattiseleifendnisi,veltemporquamvulputateultricies.Namegestasorcisitametimperdietimperdiet.Classaptenttacitisociosquadlitoratorquentperconubianostra,perinceptoshimenaeos.Craslectusmassa,sollicitudinintemporsagittis,volutpatmaximusmetus.Morbinecauctormetus.Aliquamlectusest,cursusaerossed,congueultriceselit.Aeneaneuismodgravidaaugue.Crasiaculisdiamatodiopretiummalesuada.Integergravidaeuismodpurus,asemperenimeleifendat.Praesenttemporsemidloremgravida,vulputateinterdumfelisvehicula.Sedfringillaaccumsanelementum.Sedconguelectusurna,sitamettempordiamconsecteturut.Sedaarcuquiseratfinibusporttitoretnecnulla.Duisutmollisdolor.Etiametfermentumnisl.Sedconguevariustortor.Inconsequat,quamvitaeconsequatluctus,leomisagittisnunc,tinciduntfinibuserosrisussitametnisi.Aeneanpretiumnisldiam.Vestibulumportaesteuaccumsaneuismod.Vestibulumelementumtempusconsectetur.Aeneanquismollisturpis.Vestibulumsuscipitcommododui,euultricieseroshendreritullamcorper.VestibulumanteipsumprimisinfaucibusorciluctusetultricesposuerecubiliaCurae;Phasellusfaucibusnislacquamvolutpataliquet.Crasconguesollicitudinnibh,intinciduntnibhmollisac.Fusceinvestibulumlectus.Phasellussodalesnequevitaenibhrhoncusmolestienecvelelit.Crasvelnibhturpis.Duissollicitudinaipsumidfaucibus.Integerfeugiattortorquisanteporttitor,tempusfacilisisexconsectetur.Integerexodio,pellentesqueeunisinec,bibendumsagittisneque.Fusceegetliberoquam.Maecenasquismiante.Fuscefringillarisusvelantepulvinaraliquam.Sedquisurnaatsemconsequatinterdum.Integeraccondimentumquam.Vestibulumestaugue,elementumanislsitamet,imperdietlobortisarcu.Nuncvestibulumbibendumnulla,quisefficituraugueinterdumet.Nullamsemperrisusdiam.Vestibuluminlaoreetelit.Crasfaucibusduimauris.Invitaeultriciesnunc.Utviverraacmetussitametmaximus.Nuncmollisblanditloremquisvestibulum.Namutplaceratlectus,etdictumligula.Quisqueetliberopurus.Pellentesqueutaliquetmi.Nuncnuncnunc,finibuseturnavitae,variusmolestieest.Nullafacilisi.Donecconvallisantenecporttitoraccumsan.Morbirutrum,felisquisiaculisconvallis,diamenimrutrumnisl,afaucibussapienliberoacerat.Nullamnonmassautvelitluctustristiqueegetquisnulla.Crasvariuslectusatleoeuismodtempus.Vivamussollicitudinjustoacleoaccumsan,alobortisleoimperdiet.Donecnechendreritenim,inconsequattortor.Curabituraauctorurna,athendreritnibh.Orcivariusnatoquepenatibusetmagnisdisparturientmontes,nasceturridiculusmus.Utfinibusutpurusegetaliquam.Maecenasgravidalectusligula,semperviverraauguelaoreetsitamet.Etiamnecaliquamdiam.Donecacleovelexdapibuspretium.Maecenasfaucibussedturpisatsagittis.Suspendissevestibulumdolorutsemfeugiatmattis.Phasellusdignissimsodalesdapibus.Etiamenimdiam,laciniavelullamcorperac,porttitorvitaediam.Donecipsumeros,consecteturseddictumut,pellentesquenecnulla.Duisultriciesantenecerosposuerefeugiat.Pellentesquedignissimviverraturpis,sitamettinciduntmauriseuismodut.Morbitempor,dolorhendreritconvallislacinia,odiovelitrutrumorci,avenenatissapienloremeulorem.Fuscevelitelit,ullamcorpersedlaoreetid,interdumnecnibh.Nullamgravidatortoracfermentumaliquet.Praesentluctusquamidipsumtinciduntdapibus.Phasellusnecmolestiequam.Donecjustolorem,pulvinarnonpellentesquevel,euismodidlorem.Phasellusvelnullafelis.Donecmimassa,sollicitudinegetsapienut,laciniacommodomassa.Invitaeligulaquisturpismolestieullamcorperidcondimentumturpis.Donectempusenimconvalliselitultriciesblandit.Donecsapienjusto,ullamcorpersitametduinon,portaluctusest.Maurisimperdietsemidvelitvolutpataliquam.Curabiturfelisfelis,finibussedsollicitudinnec,blanditidfelis.Vivamuspulvinarloremtortor,amollisrisustinciduntnec.Nuncimperdietsemvelmaurissuscipit,neclaoreetlectustristique.Namcondimentumquiseratataliquam.Nuncullamcorperlaoreetmassa,utmolliseroshendreritvitae.Donecpharetraenimvitaeliberodapibus,insempererosporta.Fuscejustodolor,egestaseubibendumeu,rhoncusullamcorperest.Namullamcorpermivelmaurissemperinterdum.Utsempersollicitudinerat";

        Guid referenceGuid = new Guid("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        Database db;

        [TestInitialize]
        public void SetUp()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            db = factory.Create("Db2Test");

            try
            {
                DeleteStoredProcedures();
            }
            catch { }

            CreateStoredProcedures();
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteStoredProcedures();
        }

        [TestMethod]
        public void CanInsertNullStringParameter()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            Database db = factory.Create("Db2Test");

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    string sqlString = "insert into Customers (CustomerID, CompanyName, ContactName) Values (@id, @name, @contact)";
                    DbCommand insert = db.GetSqlStringCommand(sqlString);
                    db.AddInParameter(insert, "@id", DbType.Int32, 1);
                    db.AddInParameter(insert, "@name", DbType.String, "fee");
                    db.AddInParameter(insert, "@contact", DbType.String, null);

                    db.ExecuteNonQuery(insert, transaction);
                    transaction.Rollback();
                }
            }
        }

        [TestMethod]
        public void CanSetValueForGuidParameters()
        {
            string parameterName = "dummyParameter";
            byte[] guidBytes = new byte[16];

            DbCommand dBCommand = db.GetStoredProcCommand("IGNORED");
            db.AddInParameter(dBCommand, parameterName, DbType.Guid);
            db.SetParameterValue(dBCommand, parameterName, new Guid(guidBytes));
            object paramValue = db.GetParameterValue(dBCommand, parameterName);

            Assert.IsNotNull(paramValue);
            Assert.AreSame(typeof(Guid), paramValue.GetType());
        }

        [TestMethod]
        public void CanSetValueForGuidParametersAfterRoundtripToDatabase()
        {
            Guid guid = new Guid(new byte[16]);
            object outputGuidValue = null;

            DbCommand dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 16);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid);
            db.SetParameterValue(dbCommand, "inputGuid", guid);

            db.ExecuteNonQuery(dbCommand);
            outputGuidValue = db.GetParameterValue(dbCommand, "outputGuid");

            Assert.IsNotNull(outputGuidValue);
            Assert.IsFalse(outputGuidValue == DBNull.Value);
            Assert.AreSame(typeof(Guid), outputGuidValue.GetType());
            Assert.AreEqual(guid, outputGuidValue);
        }

        [TestMethod]
        public void CanSetValueForStringParametersAfterRoundtripToDatabase()
        {
            string paramValue = "strparamvalue";
            object outputStringValue = null;

            DbCommand dbCommand = db.GetStoredProcCommand("SetAndGetString");
            db.AddOutParameter(dbCommand, "outputString", DbType.String, paramValue.Length);
            db.AddInParameter(dbCommand, "inputString", DbType.String);
            db.SetParameterValue(dbCommand, "inputString", paramValue);

            db.ExecuteNonQuery(dbCommand);
            outputStringValue = db.GetParameterValue(dbCommand, "outputString");

            Assert.IsNotNull(outputStringValue);
            Assert.IsFalse(outputStringValue == DBNull.Value);
            Assert.AreSame(typeof(String), outputStringValue.GetType());
            Assert.AreEqual(paramValue, outputStringValue);
        }

        [TestMethod]
        public void CanSetBigStringParameter()
        {
            DbCommand dbCommand = db.GetStoredProcCommand("SetAndGetBigString");
            db.AddOutParameter(dbCommand, "outputXml", DbType.Decimal, 0);
            db.AddInParameter(dbCommand, "inputXml", DbType.String, BIGSTRING);
            db.ExecuteNonQuery(dbCommand);
            int outputVal = ((int)db.GetParameterValue(dbCommand, "outputXml"));
            Assert.AreEqual(BIGSTRING.Length, outputVal);
        }

        [TestMethod]
        public void CanGetValueForDiscoveredGuidParameters()
        {
            Guid guid = new Guid(new byte[16]);
            object inputGuidValue = null;

            DbCommand dbCommand = db.GetStoredProcCommand("SetAndGetGuid", null, guid);

            inputGuidValue = db.GetParameterValue(dbCommand, "inputGuid");

            Assert.IsNotNull(inputGuidValue);
            Assert.IsFalse(inputGuidValue == DBNull.Value);
            Assert.AreSame(typeof(Guid), inputGuidValue.GetType());
            Assert.AreEqual(guid, inputGuidValue);
        }

        [TestMethod]
        public void CanUseGuidParameterMultipleTimes()
        {
            DbCommand dbCommand;
            Guid myGuid = new Guid();
            Guid outputVal;

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 16);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 16);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);
        }

        [TestMethod]
        public void SecondUseWithDifferentTypeKeepsOriginalType()
        {
            DbCommand dbCommand;
            Guid myGuid = new Guid();
            Guid outputVal;

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.Guid, 16);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);

            dbCommand = db.GetStoredProcCommand("SetAndGetGuid");
            myGuid = new Guid();
            db.AddOutParameter(dbCommand, "outputGuid", DbType.String, 16);
            db.AddInParameter(dbCommand, "inputGuid", DbType.Guid, myGuid);
            db.ExecuteNonQuery(dbCommand);
            outputVal = ((Guid)db.GetParameterValue(dbCommand, "outputGuid"));
            Assert.AreEqual(myGuid, outputVal);
        }

        void CreateStoredProcedures()
        {
            string guid_storedProcedureCreation =
@"CREATE Procedure ENTLIBTEST.SetAndGetGuid
(	
    INOUT outputGuid BINARY(16),
	IN inputGuid	BINARY(16)
)
DYNAMIC RESULT SETS 1 
LANGUAGE SQL 
NOT DETERMINISTIC 
CALLED ON NULL INPUT
SET OPTION COMMIT = *RR
BEGIN 
	SELECT inputGuid INTO outputGuid FROM SYSIBM.SYSDUMMY1;
END";
            string string_storedProcedureCreation =
@"CREATE Procedure ENTLIBTEST.SetAndGetString
(	
    INOUT outputString VARCHAR(32),
	IN  inputString VARCHAR(32)
)
DYNAMIC RESULT SETS 1 
LANGUAGE SQL 
NOT DETERMINISTIC 
CALLED ON NULL INPUT
SET OPTION COMMIT = *RR
BEGIN 
	SELECT inputString INTO outputString FROM SYSIBM.SYSDUMMY1;
END";

            string big_string_storedProcedureCreation =
@"CREATE Procedure ENTLIBTEST.SetAndGetBigString
(	
    INOUT outputXml Int,
	IN  inputXml DBCLOB CCSID 13488
)
DYNAMIC RESULT SETS 1 
LANGUAGE SQL 
NOT DETERMINISTIC 
CALLED ON NULL INPUT
SET OPTION COMMIT = *RR
BEGIN 
    SET outputXml = LENGTH(inputXml);
END";

            db.ExecuteNonQuery(CommandType.Text, guid_storedProcedureCreation);

            db.ExecuteNonQuery(CommandType.Text, string_storedProcedureCreation);

            db.ExecuteNonQuery(CommandType.Text, big_string_storedProcedureCreation);
        }

        void DeleteStoredProcedures()
        {
            db.ExecuteNonQuery(CommandType.Text, "DROP SPECIFIC PROCEDURE ENTLIBTEST.SetAndGetGuid");

            db.ExecuteNonQuery(CommandType.Text, "DROP SPECIFIC PROCEDURE ENTLIBTEST.SetAndGetString");

            db.ExecuteNonQuery(CommandType.Text, "DROP SPECIFIC PROCEDURE ENTLIBTEST.SetAndGetBigString");
        }
    }
}
