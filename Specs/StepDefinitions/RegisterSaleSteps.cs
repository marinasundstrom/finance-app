using TechTalk.SpecFlow;
using Shouldly;
using Bunit;
using Accounting.Client.Verifications;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Accounting.Client;
using NSubstitute;
using NSubstitute.Core;

namespace Accounting.Specs.StepDefinitions;

[Binding]
[Scope(Feature = "Register sale", Scenario = "Clicking a button")]
public class RegisterSaleSteps : IDisposable
{
    TestContext ctx;
    IRenderedComponent<RegisterSalePage> cut;

    public RegisterSaleSteps()
    {
        // Arrange: render the Counter.razor component
        ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        ctx.Services.AddMudServices();

        ctx.Services.AddTransient<IAccountsClient>(sp =>
        {
            var client = Substitute.For<IAccountsClient>();

            client
                .GetAccountsAsync(default, default)
                .ReturnsForAnyArgs(new [] {
                    new Client.Account {

                    }
                });

            return client;
        });

        ctx.Services.AddTransient<IVerificationsClient>(sp =>
        {
            var client = Substitute.For<IVerificationsClient>();

            client
                .CreateVerificationAsync(default)
                .Returns("V1");

            return client;
        });

        cut = ctx.RenderComponent<RegisterSalePage>();
    }

    [Given(@"I have entered a description")]
    public void GivenIHaveEnteredADescription()
    {
        cut.Find("textarea").Input("Test");
    }

    [When(@"I click the button")]
    public void WhenIClickTheButton()
    {
        // Act: find and click the <button> element to increment
        // the counter in the <p> element
        cut.Find("button").Click();
    }

    [Then(@"text gets displayed")]
    public void ThenTextGetsDisplayed()
    {
        //calculator.Result.ShouldBe(result);

        cut.Find("p");
        
        //.MarkupMatches("<p>Current count: 1</p>");
    }

    public void Dispose()
    {
        ctx.Dispose();
    }
}