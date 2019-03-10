using System.Collections.Generic;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SetLinksTelecom.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SetLinksTelecom.Data.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //base.Seed(context);

            context.Portals.AddOrUpdate(a => a.Name,
                new Portal { Name = "Jazz Cash", AccString = "22-01-0001" },
                new Portal { Name = "Efics", AccString = "22-01-0002" }
                );
            context.AccAccounts.AddOrUpdate(a => a.AccString,
                new AccAccount
                {
                    HeadCode = 22,
                    SubHeadCode = 01,
                    OID = 0,
                    AccCode = 1,
                    AccMade = 0,
                    AccName = "Jazz Cash",
                    AccString = "22-01-0001"
                },
                new AccAccount
                {
                    HeadCode = 22,
                    SubHeadCode = 01,
                    OID = 0,
                    AccCode = 2,
                    AccMade = 0,
                    AccName = "Efics",
                    AccString = "22-01-0002"
                },
                new AccAccount
                {
                    HeadCode = 14,
                    SubHeadCode = 02,
                    OID = 0,
                    AccCode = 1,
                    AccMade = 0,
                    AccName = "Business Cash",
                    AccString = "14-02-0001"
                }

           );
            //context.AccAccounts.Add(new AccAccount
            //{
            //    HeadCode = 22,
            //    SubHeadCode = 01,
            //    OID = 0,
            //    AccCode = 1,
            //    AccMade = 0,
            //    AccName = "Jazz Cash",
            //    AccString = "22-01-0001"
            //});
            //context.AccAccounts.Add(new AccAccount
            //{
            //    HeadCode = 22,
            //    SubHeadCode = 01,
            //    OID = 0,
            //    AccCode = 2,
            //    AccMade = 0,
            //    AccName = "Efics",
            //    AccString = "22-01-0002"
            //});

            context.AccTypes.AddOrUpdate(a => a.TypeName,
                new AccType
                {
                    TypeName = "ASSETS",
                    TypeCode = 1,
                    //AccHeads = new List<AccHead>().AddRange( new List<AccHead>()
                    //    )
                    AccHeads = new List<AccHead>
                    {
                        new AccHead
                        {
                            TypeCode = 1, HeadName = "Non Current Assets", HeadString = "11", HeadCode = 11,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 11,OID = 0,SubHeadCode = 1, SubHeadName = "Tangible Stock Value", SubHeadString = "11-01"},
                                new AccSubHead {HeadCode = 11,OID = 0,SubHeadCode = 2, SubHeadName = "Intangible Stock Value", SubHeadString = "11-02"}
                            }
                        },
                        new AccHead {TypeCode = 1, HeadName = "Long Term Deposits", HeadString = "12", HeadCode = 12},
                        new AccHead {TypeCode = 1, HeadName = "Current Assets", HeadString = "13", HeadCode = 13
                            //SubHeads = new List<AccSubHead>
                            //{
                            //    new AccSubHead {HeadCode = 13,OID = 0,SubHeadCode = 1, SubHeadName = "DO Customers", SubHeadString = "13-01"},
                            //    new AccSubHead {HeadCode = 13,OID = 0,SubHeadCode = 2, SubHeadName = "RO Customer", SubHeadString = "13-02"}
                            //}
                        },
                        new AccHead
                        {
                            TypeCode = 1, HeadName = "Cash & Bank Balances", HeadString = "14", HeadCode = 14,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 14,OID = 0,SubHeadCode = 1, SubHeadName = "Banks", SubHeadString = "14-01"},
                                new AccSubHead {HeadCode = 14,OID = 0,SubHeadCode = 2, SubHeadName = "Cash", SubHeadString = "14-02"},
                                new AccSubHead {HeadCode = 14,OID = 0,SubHeadCode = 3, SubHeadName = "Outstanding Receivings", SubHeadString = "14-03"}
                            }
                        }
                    }
                },
                new AccType
                {
                    TypeName = "LIABILITIES",
                    TypeCode = 2,
                    AccHeads = new List<AccHead>
                    {
                        new AccHead {TypeCode = 2, HeadName = "Long Term Liabilities", HeadString = "21", HeadCode = 21},
                        new AccHead {TypeCode = 2, HeadName = "Short Term Liabilities", HeadString = "22", HeadCode = 22,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 22,OID = 0,SubHeadCode = 1, SubHeadName = "Payable To Supplier", SubHeadString = "22-01"},
                                new AccSubHead {HeadCode = 22,OID = 0,SubHeadCode = 2, SubHeadName = "Commission Payable", SubHeadString = "22-02"},
                                new AccSubHead {HeadCode = 22,OID = 0,SubHeadCode = 3, SubHeadName = "Payable To Employees", SubHeadString = "22-03"},
                                new AccSubHead {HeadCode = 22,OID = 0,SubHeadCode = 4, SubHeadName = "Outstanding Payments", SubHeadString = "22-04"},
                            }
                        }
                    }
                },
                new AccType
                {
                    TypeName = "RESERVE & FUNDS",
                    TypeCode = 3,
                    AccHeads = new List<AccHead>
                    {
                        new AccHead {TypeCode = 3, HeadName = "Reserves & Funds", HeadString = "31", HeadCode = 31,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 31,OID = 0,SubHeadCode = 1, SubHeadName = "Reserves", SubHeadString = "31-01"}
                            }
                        }
                    }
                },
                new AccType
                {
                    TypeName = "REVENUE",
                    TypeCode = 4,
                    AccHeads = new List<AccHead>
                    {
                        new AccHead {TypeCode = 4, HeadName = "Project Revenue", HeadString = "41", HeadCode = 41,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 41,OID = 0,SubHeadCode = 1, SubHeadName = "Tangible Stock Sales", SubHeadString = "41-01"},
                                new AccSubHead {HeadCode = 41,OID = 0,SubHeadCode = 2, SubHeadName = "InTangible Stock Sales", SubHeadString = "41-02"}
                            }
                        },
                        new AccHead {TypeCode = 4, HeadName = "Purchase Discount", HeadString = "42", HeadCode = 42,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 42,OID = 0,SubHeadCode = 1, SubHeadName = "Tangible Purchase Discount", SubHeadString = "42-01"},
                                new AccSubHead {HeadCode = 42,OID = 0,SubHeadCode = 2, SubHeadName = "InTangible Purchase Discount", SubHeadString = "42-02"}
                            }
                        },
                        new AccHead {TypeCode = 4, HeadName = "Commission Revenue", HeadString = "43", HeadCode = 43,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 43,OID = 0,SubHeadCode = 1, SubHeadName = "Tangible Sale Commission", SubHeadString = "43-01"},
                                new AccSubHead {HeadCode = 43,OID = 0,SubHeadCode = 2, SubHeadName = "InTangible Sale Commission", SubHeadString = "43-02"}
                            }
                        },
                        new AccHead {TypeCode = 4, HeadName = "Contra Revenue", HeadString = "44", HeadCode = 44,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 44,OID = 0,SubHeadCode = 1, SubHeadName = "Tangible Contra Revenue", SubHeadString = "44-01"},
                                new AccSubHead {HeadCode = 44,OID = 0,SubHeadCode = 2, SubHeadName = "InTangible Contra Revenue", SubHeadString = "44-02"}
                            }
                        }
                    }
                },
                new AccType
                {
                    TypeName = "EXPENDITURE",
                    TypeCode = 5,
                    AccHeads = new List<AccHead>
                    {
                        new AccHead {TypeCode = 5, HeadName = "Administrative Expense", HeadString = "51", HeadCode = 51,
                            SubHeads = new List<AccSubHead>
                            {
                                new AccSubHead {HeadCode = 51,OID = 0,SubHeadCode = 1, SubHeadName = "Tangible Cost", SubHeadString = "51-01"},
                                new AccSubHead {HeadCode = 51,OID = 0,SubHeadCode = 2, SubHeadName = "InTangible Cost", SubHeadString = "51-02"}
                                //new AccSubHead {HeadCode = 51,OID = 0,SubHeadCode = 1, SubHeadName = "Direct Stock Purchase Expense", SubHeadString = "51-01"},
                                //new AccSubHead {HeadCode = 51,OID = 0,SubHeadCode = 2, SubHeadName = "Utilities", SubHeadString = "51-02"}
                            }
                        }
                    }
                }
                );
            context.SaveChanges();
        }
    }
}
