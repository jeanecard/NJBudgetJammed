using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Business;
using NJBudgetWBackend.Commun;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NJBudgetWBackEndTests
{
    public class BudgetProcessorTest
    {
        private readonly BudgetProcessor bProcesor = new (new AppartenanceService(), new StatusProcessor());

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Add_Operation_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = 5, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope2 = new () { Value = 5, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope3 = new () { Value = -5000, DateOperation = new DateTime(2021, 1, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);

            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant,
                out float budgetEpargne,
                out float depensePure,
                budgetExpected, ops, 1, 2021);
            Assert.True(budgetRestant == 90);
            Assert.True(budgetProvisonne == 10);
            Assert.True(budgetConsomme == 10);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 0);

            ops = new List<Operation>();
             ope1 = new Operation() { Value = 5, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
             ope2 = new Operation() { Value = 5, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            ope3 = new Operation() { Value = -5000, DateOperation = new DateTime(2021, 1, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme,
                out budgetProvisonne,
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected, ops, 1, 2021);
            Assert.True(budgetRestant == 90);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 10);
            Assert.True(budgetEpargne == 10);
            Assert.True(depensePure == 0);

        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Remove_Operation_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = -5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new () { Value = -5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope3 = new () { Value = -5000, DateOperation = new DateTime(2021, 1, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);

            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant, 
                out float _,
                out float _,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == 90);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 10);
        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Remove_Operation_But_One_Ou_Of_Range_Month_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = -5, DateOperation = new DateTime(2021, 1, 1) };
            Operation ope2 = new () { Value = -5, DateOperation = new DateTime(2021, 2, 1) };
            Operation ope3 = new () { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant,
                out float _,
                out float _,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == 95);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 5);
        }


        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Add_And_Remove_Operation_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = -5, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope2 = new () { Value = 15, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope3 = new () { Value = -20, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope4 = new () { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            ops.Add(ope4);
            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant,
                out float budgetEpargne,
                out float depensePure,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == 60);
            Assert.True(budgetProvisonne == 15);
            Assert.True(budgetConsomme == 40);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 25);

            ops = new List<Operation>();
             ope1 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
             ope2 = new Operation() { Value = 15, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
             ope3 = new Operation() { Value = -20, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            ope4 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            ops.Add(ope4);

            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme,
                out budgetProvisonne,
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected,
                ops,
                1,
                2021);
            Assert.True(budgetRestant == 60);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 40);
            Assert.True(budgetEpargne == 15);
            Assert.True(depensePure == 25);
        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_With_Epargne_Consomme_Expect_Left_Spent_Epargne_Updated()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = 100, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope2 = new () { Value = -120, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope3 = new () { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);

            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant,
                out float budgetEpargne,
                out float depensePure,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == -120);
            Assert.True(budgetProvisonne == 100);
            Assert.True(budgetConsomme == 220);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 120);

            //Suppression de la provision
            ops.RemoveAt(0);
            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme, 
                out budgetProvisonne, 
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == -20);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 120);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 120);



            ops = new List<Operation>();
             ope1 = new Operation() { Value = 100, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            ope2 = new Operation() { Value = -120, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            ope3 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);


            budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme,
                out budgetProvisonne,
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected,
                ops,
                1,
                2021);
            Assert.True(budgetRestant == -120);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 220);
            Assert.True(budgetEpargne == 100);
            Assert.True(depensePure == 120);

            //Suppression de la provision
            ops.RemoveAt(0);
            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme,
                out budgetProvisonne,
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected,
                ops,
                1,
                2021);
            Assert.True(budgetRestant == -20);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 120);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 120);

        }

        [Fact]
        public void ProcessBudgetSpentAndLeft_Epargne_Et_Depense_Expected_Budget_Expect_No_Left()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = 50, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope2 = new () { Value = -50, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            Operation ope3 = new () { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);


            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant, 
                out float budgetEpargne,
                out float depensePure,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == 0);
            Assert.True(budgetProvisonne == 50);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 50);

            Assert.True(budgetConsomme == 100);

            ops = new List<Operation>();
            ope1 = new Operation() { Value = 50, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            ope2 = new Operation() { Value = -50, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            ope3 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };


            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme,
                out budgetProvisonne,
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected,
                ops,
                1,
                2021);
            Assert.True(budgetRestant == 0);
            Assert.True(budgetEpargne == 50);
            Assert.True(budgetProvisonne == 0);
            Assert.True(budgetConsomme == 100);
            Assert.True(depensePure == 50);
        }


        [Fact]
        public void ProcessBudgetSpentAndLeft_Depense_Exeed_Budget_Expect_Left_Neg()
        {
            List<Operation> ops = new ();
            Operation ope1 = new () { Value = 50, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            Operation ope2 = new () { Value = -60, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.EpargneAndDepense };
            Operation ope3 = new () { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };

            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);

            float budgetExpected = 100;
            bProcesor.ProcessBudgetSpentAndLeft(
                out float budgetConsomme, 
                out float budgetProvisonne, 
                out float budgetRestant,
                out float budgetEpargne,
                out float depensePure,
                budgetExpected, 
                ops, 
                1, 
                2021);
            Assert.True(budgetRestant == -10);
            Assert.True(budgetEpargne == 50);
            Assert.True(budgetConsomme == 110);
            Assert.True(budgetProvisonne == 0);
            Assert.True(depensePure == 60);


            ops = new List<Operation>();
            ope1 = new Operation() { Value = 50, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense };
            ope2 = new Operation() { Value = -60, DateOperation = new DateTime(2021, 1, 1), OperationAllowed = OperationTypeEnum.ProvisionAndDepense};
            ope3 = new Operation() { Value = -5, DateOperation = new DateTime(2021, 2, 1), IsOperationSystem = true };
            
            ops.Add(ope1);
            ops.Add(ope2);
            ops.Add(ope3);
            bProcesor.ProcessBudgetSpentAndLeft(
                out budgetConsomme,
                out budgetProvisonne,
                out budgetRestant,
                out budgetEpargne,
                out depensePure,
                budgetExpected,
                ops,
                1,
                2021);
            Assert.True(budgetRestant == -10);
            Assert.True(budgetProvisonne == 50);
            Assert.True(budgetConsomme == 110);
            Assert.True(budgetEpargne == 0);
            Assert.True(depensePure == 60);

        }

        [Fact]
        public void ProcessSyntheseOperations_One_Appartenance_Multiple_Compte_Passing_Case()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();

            List<SyntheseOperationRAwDB> operations = new ();
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 1000,
                    CompteId = guid1,
                    DateOperation = new DateTime(2021, 2, 5),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 1000
                });
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 1000,
                    CompteId = guid1,
                    DateOperation = new DateTime(2021, 2, 5),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    IsOperationSystem = true,
                    Value = 1000
                }); ;
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    CompteId = guid2,
                    DateOperation = new DateTime(2021, 2, 10),
                    OperationAllowed = OperationTypeEnum.DepenseOnly,
                    Value = -200
                });
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    CompteId = guid2,
                    DateOperation = new DateTime(2021, 2, 10),
                    OperationAllowed = OperationTypeEnum.DepenseOnly,
                    Value = -150
                });
            operations.Add(
                new SyntheseOperationRAwDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    CompteId = guid2,
                    DateOperation = new DateTime(2021, 2, 10),
                    OperationAllowed = OperationTypeEnum.DepenseOnly,
                    IsOperationSystem = true,
                    Value = -1050
                });


            List<GroupRawDB> groups = new () {
                new GroupRawDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 500,
                    Id = guid2,
                    OperationAllowed = OperationTypeEnum.DepenseOnly
                },
                new GroupRawDB()
                {
                    AppartenanceId = Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID),
                    BudgetExpected = 1000,
                    Id = guid1,
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense
                }
                };

            var result = bProcesor.ProcessSyntheseOperations(operations, groups, 2, 2021);
            Assert.NotEmpty(result.Data);
            SyntheseDepenseGlobalModelItem item1 = result.Data.ToList()[0];
            Assert.True(item1.AppartenanceId == Guid.Parse(Constant.APPARTENANCE_COMMUN_GUID));
            Assert.True(item1.BudgetValuePrevu == 1500);
            Assert.True(item1.BudgetValueDepense == 1350);
            Assert.True((int)item1.BudgetPourcentageDepense == 90);
            Assert.True(item1.Status == CompteStatusEnum.Good);
            Assert.True(result.Status == CompteStatusEnum.Good);
        }
    }
}
