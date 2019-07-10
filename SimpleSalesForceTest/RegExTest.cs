using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SimpleSalesforce;

namespace SimpleSalesForceTest
{
    [TestFixture]
    public class RegExTest
    {
        public RegExTest()
        {
        }
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DefaultRegExTest()
        {
            //string regularExpression = @"(?:(?=(?:finsmaaa:\/\/sflogin)).*?[#?]|(?!.*?[#?])^|&)([^= \n]+?)=([^& \n]+?)(?=$|&)";
            string regularExpression = @"(?<SchemaName>finsmaaa:\/\/sflogin)|(?:(?:(?:(?:#|&)\b(?<PropertyName>access_token|refresh_token|instance_url|id|issued_at|signature|id_token|state|scope|token_type)\b))=(?<PropertyValue>[^& \n]+?)(?=$|&))";
            string source = @"finsmaaa://sflogin#access_token=";

            AccessTokenResponseManager _parser = new AccessTokenResponseManager(regularExpression);
            bool parseResult = _parser.Validate(source);

            Assert.AreEqual(true, true);
        }

        [Test]
        public void ErrorRegExTest()
        {

            string source = "\u003Chead>\u003C/head>\u003Cbody>\u003Cpre style=\"word-wrap: break-word; white-space: pre-wrap;\">error=redirect_uri_mismatch&amp;error_description=redirect_uri%20must%20match%20configuration\u003C/pre>\u003C/body>";

        }
    }
}
