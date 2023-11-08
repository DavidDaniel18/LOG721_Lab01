using NUnit.Framework;
using System;
using System.IO;


namespace Data
{
    [TestFixture]
    public class CSVFactoryTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateGroupFromCSVFile_Test()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string group_csv_file_path = Path.Combine(projectDirectory, "groups.csv");

            List<Group> groups = CSVFactory.CreateGroupFromCSVFile(group_csv_file_path);
            List<Group> expected_groups = new List<Group>
            {
                new Group(1, new Data(50, 100)),
                new Group(2, new Data(100, 150)),
                new Group(3, new Data(150, 200)),
                new Group(4, new Data(200, 250)),
                new Group(5, new Data(250, 300))
            };

            CollectionAssert.AreEqual(expected_groups, groups);
        }


        [Test]
        public void CreateDataFromCSVFile_Test()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string data_csv_file_path = Path.Combine(projectDirectory, "data.csv");

            List<Data> datas = CSVFactory.CreateDataFromCSVFile(data_csv_file_path);
            List<Data> expected_datas = new List<Data>
            {
                new Data(104, 162     ),
                new Data(291, 349     ),
                new Data(271, 329     ),
                new Data(282, 340     ),
                new Data(255, 313     ),
                new Data(154, 212     ),
                new Data(116, 174     ),
                new Data(117, 175     ),
                new Data(131, 189     ),
                new Data(275, 333     ),
                new Data(127, 185     ),
                new Data(201, 259     ),
                new Data(194, 252     ),
                new Data(204, 262     ),
                new Data(159, 217     ),
                new Data(273, 331     ),
                new Data(232, 290     ),
                new Data(150, 208     ),
                new Data(195, 253     ),
                new Data(275, 333     ),
                new Data(212, 270     ),
                new Data(260, 318     ),
                new Data(168, 226     ),
                new Data(105, 163     ),
                new Data(142, 200     ),
                new Data(229, 287     ),
                new Data(204, 262     ),
                new Data(95, 153      ),
                new Data(169, 227     ),
                new Data(299, 357     ),
                new Data(253, 311     ),
                new Data(119, 177     ),
                new Data(204, 262     ),
                new Data(59, 117      ),
                new Data(271, 329     ),
                new Data(127, 185     ),
                new Data(213, 271     ),
                new Data(81, 139      ),
                new Data(209, 267     ),
                new Data(228, 286     ),
                new Data(185, 243     ),
                new Data(239, 297     ),
                new Data(114, 172     ),
                new Data(283, 341     ),
                new Data(73, 131      ),
                new Data(134, 192     ),
                new Data(236, 294     ),
                new Data(295, 353     ),
                new Data(132, 190     ),
                new Data(201, 259     ),
                new Data(248, 306     ),
                new Data(214, 272     ),
                new Data(64, 122      ),
                new Data(187, 245     ),
                new Data(226, 284     ),
                new Data(199, 257     ),
                new Data(111, 169     ),
                new Data(156, 214     ),
                new Data(89, 147      ),
                new Data(274, 332     ),
                new Data(54, 112      ),
                new Data(171, 229     ),
                new Data(239, 297     ),
                new Data(195, 253     ),
                new Data(230, 288     ),
                new Data(281, 339     ),
                new Data(255, 313     ),
                new Data(174, 232     ),
                new Data(253, 311     ),
                new Data(219, 277     ),
                new Data(155, 213     ),
                new Data(233, 291     ),
                new Data(251, 309     ),
                new Data(108, 166     ),
                new Data(143, 201     ),
                new Data(86, 144      ),
                new Data(248, 306     ),
                new Data(87, 145      ),
                new Data(104, 162     ),
                new Data(225, 283     ),
                new Data(67, 125      ),
                new Data(121, 179     ),
                new Data(195, 253     ),
                new Data(159, 217     ),
                new Data(111, 169     ),
                new Data(133, 191     ),
                new Data(133, 191     ),
                new Data(294, 352     ),
                new Data(83, 141      ),
                new Data(183, 241     ),
                new Data(247, 305     ),
                new Data(219, 277     ),
                new Data(86, 144      ),
                new Data(83, 141      ),
                new Data(172, 230     ),
                new Data(231, 289     ),
                new Data(169, 227     ),
                new Data(245, 303     ),
                new Data(225, 283     ),
                new Data(197, 255     ),
            };

            CollectionAssert.AreEqual(expected_datas, datas);
        }
    }
}