using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Business;
using System;
using System.Collections.Generic;
using Xunit;

namespace NJBudgetWBackEndTests
{
    public class BalanceProcessorTest
    {
        [Fact]
        public void ProcessBalance_Epargne_NormalCase_Expect_Epargne_OK()
        {
            BalanceProcessor buProcessor = new ();
            List<IOperation> operations = new ()
            {
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -300
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -200,
                    IsOperationSystem = true
                },

                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 200
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 900
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -100
                }
            };
            buProcessor.ProcessBalance(out float result, 1000, operations, new DateTime(2021, 2, 15));
            Assert.Equal(700, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 3, 15));
            Assert.Equal(1500, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 4, 15));
            Assert.Equal(1500, result);

            buProcessor.ProcessBalance(out result, 1000, operations, null);
            Assert.Equal(1500, result);

        }
        [Fact]
        public void ProcessBalance_Provision_NormalCase_Expect_Epargne_OK()
        {
            BalanceProcessor buProcessor = new ();
            List<IOperation> operations = new ()
            {
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.ProvisionAndDepense,
                    Value = -500
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.ProvisionAndDepense,
                    Value = 200
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,1),
                    OperationAllowed = OperationTypeEnum.ProvisionAndDepense,
                    Value = 900
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.ProvisionAndDepense,
                    Value = -100
                }
            };
            buProcessor.ProcessBalance(out float result, 1000, operations, new DateTime(2021, 2, 15));
            Assert.Equal(700, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 3, 15));
            Assert.Equal(1500, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 4, 15));
            Assert.Equal(1500, result);

            buProcessor.ProcessBalance(out result, 1000, operations, null);
            Assert.Equal(1500, result);

        }

        [Fact]
        public void ProcessBalance_Provision_With_Too_Much_Provision_Expect_Epargne_OK()
        {
            BalanceProcessor buProcessor = new ();

            List<IOperation> operations = new ()
            {
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -500
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 2000
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 900
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    IsOperationSystem = true,
                    Value = -25
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    IsOperationSystem = false,
                    Value = -25
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    IsOperationSystem = false,
                    Value = -50
                }
                , //+2300
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 3,5),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 4500
                }
            };
            buProcessor.ProcessBalance(out float result, 1000, operations, new DateTime(2021, 2, 15));
            Assert.Equal(2500, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 2, 25));
            Assert.Equal(2400, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 3, 1));
            Assert.Equal(3300, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 3, 20));
            Assert.Equal(6800, result);
        }

        [Fact]
        public void ProcessBalance_Provision_With_Too_Much_Depense_Expect_Epargne_OK()
        {
            BalanceProcessor buProcessor = new ();
            List<IOperation> operations = new ()
            {
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -500
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -2000
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 900
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    IsOperationSystem = false,
                    Value = -50
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,20),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    IsOperationSystem = true,
                    Value = -50
                }
                , //+2300
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 3,5),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -4500
                }
            };
            buProcessor.ProcessBalance(out float result, 1000, operations, new DateTime(2021, 2, 15));
            Assert.Equal(-1500, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 2, 25));
            Assert.Equal(-1600, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 3, 1));
            Assert.Equal(-700, result);

            buProcessor.ProcessBalance(out result, 1000, operations, new DateTime(2021, 3, 20));
            Assert.Equal(-5200, result);
        }


        [Fact]
        public void ProcessBalance_Provision_On_Previoos_Month_Expect_Balance_OK()
        {
            BalanceProcessor buProcessor = new ();
            List<IOperation> operations = new ()
            {
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 1,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = 244
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,1),
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -180
                },
                new BasicOperation()
                {
                    DateOperation = new DateTime(2021, 2,1),
                    IsOperationSystem = true,
                    OperationAllowed = OperationTypeEnum.EpargneAndDepense,
                    Value = -100
                }
            };
            buProcessor.ProcessBalance(out float result, 280, operations, new DateTime(2021, 2, 15));
            Assert.Equal(244, result);

        }


    }
}
