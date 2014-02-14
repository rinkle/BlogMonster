using System;
using System.Linq;
using System.ServiceModel.Syndication;
using BlogMonster.Configuration;
using BlogMonster.Extensions;
using NUnit.Framework;
using Shouldly;

namespace BlogMonster.Tests
{
    [TestFixture]
    public class WhenLoadingOneSimpleBlogPostWithOneImage : TestFor<IEmbeddedSyndicationFeedSource>
    {
        private const string _someImageControllerPath = "/Some/Image";

        protected override IEmbeddedSyndicationFeedSource GivenSubject()
        {
            return BlogMonsterBuilder.FromEmbeddedResources(GetType().Assembly)
                                     .WithResourceNameFilter(s => s.Contains(".SinglePostWithImage.") && s.EndsWith(".markdown"))
                                     .WithRssSettings(new RssFeedSettings("feedId",
                                                                          "title",
                                                                          "description",
                                                                          new SyndicationPerson(),
                                                                          "http://www.example.com/image.jpg",
                                                                          "language",
                                                                          "copyright",
                                                                          new Uri("http://www.example.com")))
                                     .WithBaseUris(new Uri("http://www.example.com"), new Uri("http://www.example.com/" + _someImageControllerPath + "/"))
                                     .Grr();
        }

        protected override void When()
        {
        }

        [Test]
        public void ThereShouldBeOneBlogPost()
        {
            Subject.Feed.Items.Count().ShouldBe(1);
        }

        [Test]
        public void ThereShouldBeARelativeLinkToAnImageControllerAction()
        {
            Subject.Feed.Items.Single().Content.ToHtml().ShouldContain(_someImageControllerPath);
        }
    }
}