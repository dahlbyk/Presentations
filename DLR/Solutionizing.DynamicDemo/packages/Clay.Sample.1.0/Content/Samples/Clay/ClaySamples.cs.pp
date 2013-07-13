using System;
using ClaySharp;

namespace $rootnamespace$.Samples.Clay {
    public static class ClaySamples {
        public static void RunAll() {
            PropertyAssignment();
            IndexedPropertySyntax();
            ChainableSetters();
            AnonymousObject();
            NamedArguments();
            ArrayCreation();
            ArrayCreationOnExistingObject();
            ComplexTest();
            CastToCLRInterface();
        }

        public static void PropertyAssignment() {
            Console.WriteLine("**** PropertyAssignment ****");

            dynamic New = new ClayFactory();

            var person = New.Person();
            person.FirstName = "Louis";
            person.LastName = "Dejardin";

            Console.WriteLine("{0} {1}", person.FirstName, person.LastName);

            // This is equivalent, using method call syntax
            Console.WriteLine("{0} {1}", person.FirstName(), person.LastName());

            Console.WriteLine();
        }

        public static void IndexedPropertySyntax() {
            Console.WriteLine("**** IndexedPropertySyntax ****");

            dynamic New = new ClayFactory();

            var person = New.Person();
            person["FirstName"] = "Louis";
            person["LastName"] = "Dejardin";

            // Note that you can mix and match
            Console.WriteLine("{0} {1}", person["FirstName"], person.LastName);

            Console.WriteLine();
        }

        public static void ChainableSetters() {
            Console.WriteLine("**** ChainableSetters ****");
            dynamic New = new ClayFactory();

            var person = New.Person()
                .FirstName("Louis")
                .LastName("Dejardin");

            Console.WriteLine("{0} {1}", person.FirstName, person.LastName);

            Console.WriteLine();
        }

        public static void AnonymousObject() {
            Console.WriteLine("**** AnonymousObject ****");

            dynamic New = new ClayFactory();

            var person = New.Person(new {
                FirstName = "Louis",
                LastName = "Dejardin"
            });

            Console.WriteLine("{0} {1}", person.FirstName, person.LastName);

            Console.WriteLine();
        }

        public static void NamedArguments() {
            Console.WriteLine("**** NamedArguments ****");

            dynamic New = new ClayFactory();

            var person = New.Person(
                FirstName: "Louis",
                LastName: "Dejardin"
            );

            Console.WriteLine("{0} {1}", person.FirstName, person.LastName);

            Console.WriteLine();
        }

        public static void ArrayCreation() {
            Console.WriteLine("**** ArrayCreation ****");

            dynamic New = new ClayFactory();

            var people = New.Array(
                New.Person().FirstName("Louis").LastName("Dejardin"),
                New.Person().FirstName("Bertrand").LastName("Le Roy")
            );

            Console.WriteLine("Count = {0}", people.Count);
            Console.WriteLine("people[0].FirstName = {0}", people[0].FirstName);

            foreach (var person in people) {
                Console.WriteLine("{0} {1}", person.FirstName, person.LastName);
            }

            Console.WriteLine();
        }

        public static void ArrayCreationOnExistingObject() {
            Console.WriteLine("**** ArrayCreationOnExistingObject ****");

            dynamic New = new ClayFactory();

            var person = New.Person(
                FirstName: "Bertrand",
                LastName: "Le Roy"
            );

            person.Aliases("bleroy", "BoudinFatal");

            // Array size is dynamic
            person.Aliases.Add("One more alias");

            Console.WriteLine("{0} {1}", person.FirstName, person.LastName);
            foreach (var alias in person.Aliases) {
                Console.WriteLine("    {0}", alias);
            }

            Console.WriteLine();
        }

        public static void ComplexTest() {
            Console.WriteLine("**** ComplexTest ****");

            dynamic New = new ClayFactory();

            var directory = New.Array(
                New.Person(
                    FirstName: "Louis",
                    LastName: "Dejardin",
                    Aliases: new[] { "Lou" }
                ),
                New.Person(
                    FirstName: "Bertrand",
                    LastName: "Le Roy"
                ).Aliases("bleroy", "boudin"),
                New.Person(
                    FirstName: "Renaud",
                    LastName: "Paquay"
                ).Aliases("Your Scruminess", "Chef")
            ).Name("Some Orchard folks");

            foreach (var person in directory) {
                Console.WriteLine("{0} {1}", person.FirstName, person.LastName);
                foreach (var alias in person.Aliases) {
                    Console.WriteLine("    {0}", alias);
                }
            }

            Console.WriteLine();
        }

        public interface IPerson {
            string FirstName { get; set; }
            string LastName { get; set; }
        }

        public static void CastToCLRInterface() {
            Console.WriteLine("**** CastToCLRInterface ****");

            dynamic New = new ClayFactory();

            var person = New.Person();
            person.FirstName = "Louis";
            person.LastName = "Dejardin";

            // Concrete interface implementation gets magically created!
            IPerson lou = person;

            // You get intellisense and compile time check here
            Console.WriteLine("{0} {1}", lou.FirstName, lou.LastName);

            Console.WriteLine();
        }
    }
}
