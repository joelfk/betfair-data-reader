namespace Betfair.Tests

open Betfair.LoginSerialisation
open FsCheck
open NUnit.Framework

[<TestFixture>]
module LoginSerialisation =
        
    [<Test>]
    let ``When mapLoginStatus is called with status text that is mapped, it returns the corresponding LoginStatus`` () =
        Assert.AreEqual(Success, mapLoginStatus "SUCCESS")
        Assert.AreEqual(InvalidUserNameOrPassword, mapLoginStatus "INVALID_USERNAME_OR_PASSWORD")
        Assert.AreEqual(AccountNowLocked, mapLoginStatus "ACCOUNT_NOW_LOCKED")
        Assert.AreEqual(AccountAlreadyLocked, mapLoginStatus "ACCOUNT_ALREADY_LOCKED")
        Assert.AreEqual(PendingAuthentication, mapLoginStatus "PENDING_AUTH")
        Assert.AreEqual(TelbetTermsAndConditionsRejected, mapLoginStatus "TELBET_TERMS_CONDITIONS_NA")
        Assert.AreEqual(DuplicateCards, mapLoginStatus "DUPLICATE_CARDS")
        Assert.AreEqual(SecurityQuestionIncorrectThreeTimes, mapLoginStatus "SECURITY_QUESTION_WRONG_3X")
        Assert.AreEqual(IdentityVerificationProcessSuspended, mapLoginStatus "KYC_SUSPEND")
        Assert.AreEqual(AccountSuspended, mapLoginStatus "SUSPENDED")
        Assert.AreEqual(AccountClosed, mapLoginStatus "CLOSED")
        Assert.AreEqual(AccountSelfExcluded, mapLoginStatus "SELF_EXCLUDED")
        Assert.AreEqual(DkRegulatorInaccessible, mapLoginStatus "INVALID_CONNECTIVITY_TO_REGULATOR_DK")
        Assert.AreEqual(NotAuthorisedByDkRegulator, mapLoginStatus "NOT_AUTHORIZED_BY_REGULATOR_DK")
        Assert.AreEqual(ItRegulatorInaccessible, mapLoginStatus "INVALID_CONNECTIVITY_TO_REGULATOR_IT")
        Assert.AreEqual(NotAuthorisedByItRegulator, mapLoginStatus "NOT_AUTHORIZED_BY_REGULATOR_IT")
        Assert.AreEqual(SecurityRestrictedLocation, mapLoginStatus "SECURITY_RESTRICTED_LOCATION")
        Assert.AreEqual(BettingRestrictedLocation, mapLoginStatus "BETTING_RESTRICTED_LOCATION")
        Assert.AreEqual(TradingMasterAccount, mapLoginStatus "TRADING_MASTER")
        Assert.AreEqual(TradingMasterAccountSuspended, mapLoginStatus "TRADING_MASTER_SUSPENDED")
        Assert.AreEqual(AgentClientMaster, mapLoginStatus "AGENT_CLIENT_MASTER")
        Assert.AreEqual(AgentClientMasterSuspended, mapLoginStatus "AGENT_CLIENT_MASTER_SUSPENDED")
        Assert.AreEqual(DanishAuthorizationRequired, mapLoginStatus "DANISH_AUTHORIZATION_REQUIRED")
        Assert.AreEqual(SpainMigrationRequired, mapLoginStatus "SPAIN_MIGRATION_REQUIRED")
        Assert.AreEqual(DenmarkMigrationRequired, mapLoginStatus "DENMARK_MIGRATION_REQUIRED")
        Assert.AreEqual(SpanishTermsAcceptanceRequired, mapLoginStatus "SPANISH_TERMS_ACCEPTANCE_REQUIRED")
        Assert.AreEqual(ItalianContractAcceptanceRequired, mapLoginStatus "ITALIAN_CONTRACT_ACCEPTANCE_REQUIRED")
        Assert.AreEqual(CertificateAuthenticationRequired, mapLoginStatus "CERT_AUTH_REQUIRED")
        Assert.AreEqual(PasswordChangeRequired, mapLoginStatus "CHANGE_PASSWORD_REQUIRED")
        Assert.AreEqual(PersonalMessageRequired, mapLoginStatus "PERSONAL_MESSAGE_REQUIRED")
        Assert.AreEqual(InternationalTermsAcceptanceRequired, mapLoginStatus "INTERNATIONAL_TERMS_ACCEPTANCE_REQUIRED")
        Assert.AreEqual(EmailLoginNotAllowed, mapLoginStatus "EMAIL_LOGIN_NOT_ALLOWED")
        Assert.AreEqual(MultipleUsersWithSameCredentials, mapLoginStatus "MULTIPLE_USERS_WITH_SAME_CREDENTIAL")
        Assert.AreEqual(AccountPendingPasswordChange, mapLoginStatus "ACCOUNT_PENDING_PASSWORD_CHANGE")
        Assert.AreEqual(TooManyRequests, mapLoginStatus "TEMPORARY_BAN_TOO_MANY_REQUESTS")

    [<Test>]
    let ``When mapLoginStatus is called with status text that isn't mapped, it returns OtherError`` () =
        let mapLoginStatusWithOtherStatus s = not (loginStatusMap.ContainsKey(s)) ==> lazy (mapLoginStatus s = OtherError)
        Check.QuickThrowOnFailure mapLoginStatusWithOtherStatus

    [<Test>]
    let ``When mapSessionToken is called with null, it returns None`` () =
        Assert.AreEqual(None, mapSessionToken null)

    [<Test>]
    let ``When mapSessionToken is called with an empty string, it returns None`` () =
        Assert.AreEqual(None, mapSessionToken "")

    [<Test>]
    let ``When mapSessionToken is called with neither null or an empty string, it returns a SessionToken with the given value`` () =
        let mapSessionTokenWithNonNullOrEmptySessionToken s = not (System.String.IsNullOrEmpty(s)) ==> lazy (mapSessionToken s = Some (SessionToken s))
        Check.QuickThrowOnFailure mapSessionTokenWithNonNullOrEmptySessionToken

    [<Test>]
    let ``When deserializeLoginResponseBody is called, return a LoginResponse representing the given string`` () =
        let mockloginStatusMapper _ = Success
        let mocksessionTokenMapper _ = None
        Assert.AreEqual(
            (Success, None) |> LoginResponse,
            deserializeLoginResponseBody mockloginStatusMapper mocksessionTokenMapper """ { "loginStatus": "Sample LoginStatus", "sessionToken": "Sample SessionToken" } """)