using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Repositories.Interface;
using NJBudgetWBackend.Services;
using NJBudgetWBackend.Services.Interface;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace NJBudgetWBackEndTests
{
    public class OperationServiceTest
    {

        [Fact]
        public async void RemoveAsync_With_BudgetLeft_Enough_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 500,
                Balance = 2000
            });

            OperationService opService = new (repo, gService);

            Operation ope = new ()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new (removeTask.Result);

            Assert.True(opes.Count == 1);
            Assert.True(opes[0].Value == -200);
        }
        [Fact]
        public async void RemoveAsync_With_No_Balance_And_Not_Enough_BudgetLeft_Enough_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 75,
                Balance = -100 //bizarre mais possible :-)
            });

            OperationService opService = new (repo, gService);

            Operation ope = new ()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new (removeTask.Result);

            Assert.True(opes.Count == 2);
            Assert.True(Math.Abs(opes[0].Value) == 75
                || Math.Abs(opes[0].Value) == 125);
            Assert.True(Math.Abs(opes[1].Value) == 75
                || Math.Abs(opes[1].Value) == 125);

            Assert.True(opes[0].IsOperationSystem == false
                && opes[1].IsOperationSystem == false
                );
        }

        [Fact]
        public async void RemoveAsync_With_NotEnough_Balance_And_Not_Enough_BudgetLeft_Enough_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 70,
                Balance = 50
            });

            OperationService opService = new (repo, gService);

            Operation ope = new ()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new (removeTask.Result);

            Assert.True(opes.Count == 2);
            Assert.True(
                Math.Abs(opes[0].Value) == 70
                || Math.Abs(opes[0].Value) == 130
                );
            Assert.True(
                Math.Abs(opes[1].Value) == 70
                || Math.Abs(opes[1].Value) == 130
                );

            ushort nbSystemOperation = 0;
            foreach (IOperation iter in opes)
            {
                if (iter.IsOperationSystem)
                {
                    nbSystemOperation++;
                }
            }
            Assert.True(nbSystemOperation == 0);
        }


        [Fact]
        public async void RemoveAsync_With_NotEnough_Balance_ToComplete_And_Not_Enough_BudgetLeft_Enough_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 70,
                Balance = 90
            });

            OperationService opService = new(repo, gService);

            Operation ope = new()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new(removeTask.Result);

            Assert.True(opes.Count == 3);
            Assert.True(
                Math.Abs(opes[0].Value) == 70
                || Math.Abs(opes[0].Value) == 110
                || Math.Abs(opes[0].Value) == 20
                );
            Assert.True(
                Math.Abs(opes[1].Value) == 70
                || Math.Abs(opes[1].Value) == 110
                || Math.Abs(opes[1].Value) == 20
                );
            Assert.True(
                Math.Abs(opes[2].Value) == 70
                || Math.Abs(opes[2].Value) == 110
                || Math.Abs(opes[2].Value) == 20
                );


            ushort nbSystemOperation = 0;
            foreach (IOperation iter in opes)
            {
                if (iter.IsOperationSystem)
                {
                    nbSystemOperation++;
                    Assert.True(Math.Abs(iter.Value) == 20);
                    Assert.True(iter.IsOperationSystem == true);
                }
                else
                {
                    Assert.True(iter.IsOperationSystem == false);
                }
            }
            Assert.True(nbSystemOperation == 1);
        }




        [Fact]
        public async void RemoveAsync_With_Enough_Balance_And_Not_Enough_BudgetLeft_Enough_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 70,
                Balance = 250
            });

            OperationService opService = new(repo, gService);

            Operation ope = new()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new(removeTask.Result);

            Assert.True(opes.Count == 2);
            Assert.True(
                Math.Abs(opes[0].Value) == 70
                || Math.Abs(opes[0].Value) == 130
                );
            Assert.True(
                Math.Abs(opes[1].Value) == 70
                || Math.Abs(opes[1].Value) == 130
                );

            ushort nbSystemOperation = 0;
            foreach (IOperation iter in opes)
            {
                if (iter.IsOperationSystem)
                {
                    nbSystemOperation++;
                    Assert.True(Math.Abs(iter.Value) == 130);
                }
            }
            Assert.True(nbSystemOperation == 1);
        }


        [Fact]
        public async void RemoveAsync_With_NoBudgetLeft_And_No_Balance_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 0,
                Balance = 0
            });

            OperationService opService = new (repo, gService);

            Operation ope = new ()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new (removeTask.Result);

            Assert.True(opes.Count == 1);
            Assert.True(opes[0].Value == -200);
            Assert.True(opes[0].IsOperationSystem == false);
        }

        [Fact]
        public async void RemoveAsync_With_NoBudgetLeft_And_With_Balance_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 0,
                Balance = 75
            });


            OperationService opService = new (repo, gService);

            Operation ope = new ()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new (removeTask.Result);

            Assert.True(opes.Count == 2);
            Assert.True(Math.Abs(opes[0].Value) == 125 ||Math.Abs(opes[0].Value) == 75);
            Assert.True(Math.Abs(opes[1].Value) == 125 || Math.Abs(opes[1].Value) == 75);

            ushort nbSystemOperation = 0;
            foreach (IOperation iter in opes)
            {
                if (iter.IsOperationSystem)
                {
                    nbSystemOperation++;
                }
            }
            Assert.True(nbSystemOperation == 1);
        }


        [Fact]
        public async void RemoveAsync_With_Balance_Very_Low_And_Not_Enough_BudgetLeft_Enough_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 1000,
                BudgetLeft = 75,
                Balance = 10 
            });

            OperationService opService = new(repo, gService);

            Operation ope = new()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -200
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new(removeTask.Result);

            Assert.True(opes.Count == 2);
            Assert.True(Math.Abs(opes[0].Value) == 75
                || Math.Abs(opes[0].Value) == 125);
            Assert.True(Math.Abs(opes[1].Value) == 75
                || Math.Abs(opes[1].Value) == 125);

            Assert.True(opes[0].IsOperationSystem == false
                && opes[1].IsOperationSystem == false
                );
        }

        [Fact]
        public async void RemoveAsync_With_Balance_Negative_And_Not_BudgetLeft_15_Expect_OK()
        {
            var repo = Substitute.For<IOperationsRepository>();
            var gService = Substitute.For<IGroupService>();

            //Set a return value:
            gService.GetCompteAsync(Arg.Any<Guid>(), Arg.Any<byte>(), Arg.Any<ushort>()).Returns(new Compte()
            {
                BudgetExpected = 200,
                BudgetLeft = 15,
                Balance = -95
            });

            OperationService opService = new(repo, gService);

            Operation ope = new()
            {
                DateOperation = DateTime.Now,
                IsOperationSystem = false,
                Value = -20
            };
            var removeTask = opService.RemoveAsync(ope);
            await removeTask;
            List<IOperation> opes = new(removeTask.Result);

            Assert.True(opes.Count == 2);
            Assert.True(Math.Abs(opes[0].Value) == 15
                || Math.Abs(opes[0].Value) == 5);
            Assert.True(Math.Abs(opes[1].Value) == 15
                || Math.Abs(opes[1].Value) == 5);

            Assert.True(opes[0].IsOperationSystem == false
                && opes[1].IsOperationSystem == false
                );
        }



    }



}
