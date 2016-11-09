namespace Betfair

open System.Diagnostics.CodeAnalysis
open FSharp.Data

module LoginSerialisation =
    type LoginStatus = 
        | Success
        | InvalidUserNameOrPassword
        | AccountNowLocked
        | AccountAlreadyLocked
        | PendingAuthentication
        | TelbetTermsAndConditionsRejected
        | DuplicateCards
        | SecurityQuestionIncorrectThreeTimes
        | IdentityVerificationProcessSuspended
        | AccountSuspended
        | AccountClosed
        | AccountSelfExcluded
        | DkRegulatorInaccessible
        | NotAuthorisedByDkRegulator
        | ItRegulatorInaccessible
        | NotAuthorisedByItRegulator
        | SecurityRestrictedLocation
        | BettingRestrictedLocation
        | TradingMasterAccount
        | TradingMasterAccountSuspended
        | AgentClientMaster
        | AgentClientMasterSuspended
        | DanishAuthorizationRequired
        | SpainMigrationRequired
        | DenmarkMigrationRequired
        | SpanishTermsAcceptanceRequired
        | ItalianContractAcceptanceRequired
        | CertificateAuthenticationRequired
        | PasswordChangeRequired
        | PersonalMessageRequired
        | InternationalTermsAcceptanceRequired
        | EmailLoginNotAllowed
        | MultipleUsersWithSameCredentials
        | AccountPendingPasswordChange
        | TooManyRequests
        | OtherError

    type SessionToken = SessionToken of string

    type LoginResponse = LoginResponse of LoginStatus * (SessionToken option)

    type LoginResponseJsonProvider = JsonProvider<""" { "loginStatus": "Sample LoginStatus", "sessionToken": "Sample SessionToken" } """>

    let loginStatusMap = [
        "SUCCESS", Success;
        "INVALID_USERNAME_OR_PASSWORD", InvalidUserNameOrPassword;
        "ACCOUNT_NOW_LOCKED", AccountNowLocked;
        "ACCOUNT_ALREADY_LOCKED", AccountAlreadyLocked;
        "PENDING_AUTH", PendingAuthentication;
        "TELBET_TERMS_CONDITIONS_NA", TelbetTermsAndConditionsRejected;
        "DUPLICATE_CARDS", DuplicateCards;
        "SECURITY_QUESTION_WRONG_3X", SecurityQuestionIncorrectThreeTimes;
        "KYC_SUSPEND", IdentityVerificationProcessSuspended;
        "SUSPENDED", AccountSuspended;
        "CLOSED", AccountClosed;
        "SELF_EXCLUDED", AccountSelfExcluded;
        "INVALID_CONNECTIVITY_TO_REGULATOR_DK", DkRegulatorInaccessible;
        "NOT_AUTHORIZED_BY_REGULATOR_DK", NotAuthorisedByDkRegulator;
        "INVALID_CONNECTIVITY_TO_REGULATOR_IT", ItRegulatorInaccessible;
        "NOT_AUTHORIZED_BY_REGULATOR_IT", NotAuthorisedByItRegulator;
        "SECURITY_RESTRICTED_LOCATION", SecurityRestrictedLocation;
        "BETTING_RESTRICTED_LOCATION", BettingRestrictedLocation;
        "TRADING_MASTER", TradingMasterAccount;
        "TRADING_MASTER_SUSPENDED", TradingMasterAccountSuspended;
        "AGENT_CLIENT_MASTER", AgentClientMaster;
        "AGENT_CLIENT_MASTER_SUSPENDED", AgentClientMasterSuspended;
        "DANISH_AUTHORIZATION_REQUIRED", DanishAuthorizationRequired;
        "SPAIN_MIGRATION_REQUIRED", SpainMigrationRequired;
        "DENMARK_MIGRATION_REQUIRED", DenmarkMigrationRequired;
        "SPANISH_TERMS_ACCEPTANCE_REQUIRED", SpanishTermsAcceptanceRequired;
        "ITALIAN_CONTRACT_ACCEPTANCE_REQUIRED", ItalianContractAcceptanceRequired;
        "CERT_AUTH_REQUIRED", CertificateAuthenticationRequired;
        "CHANGE_PASSWORD_REQUIRED", PasswordChangeRequired;
        "PERSONAL_MESSAGE_REQUIRED", PersonalMessageRequired;
        "INTERNATIONAL_TERMS_ACCEPTANCE_REQUIRED", InternationalTermsAcceptanceRequired;
        "EMAIL_LOGIN_NOT_ALLOWED", EmailLoginNotAllowed;
        "MULTIPLE_USERS_WITH_SAME_CREDENTIAL", MultipleUsersWithSameCredentials;
        "ACCOUNT_PENDING_PASSWORD_CHANGE", AccountPendingPasswordChange;
        "TEMPORARY_BAN_TOO_MANY_REQUESTS", TooManyRequests] |> Map.ofList

    let mapLoginStatus s =
        match Map.tryFind s loginStatusMap with
        | Some x -> x
        | None -> OtherError

    let mapSessionToken x =
        match x with
        | null -> None
        | "" -> None
        | x -> Some (x |> SessionToken)
        
    let deserializeLoginResponseBody loginStatusMapper sessionTokenMapper body =
        let x = LoginResponseJsonProvider.Parse(body)
        (loginStatusMapper x.LoginStatus, sessionTokenMapper x.SessionToken) |> LoginResponse

    [<ExcludeFromCodeCoverage>]
    let loginResponseDeserializer =
        deserializeLoginResponseBody mapLoginStatus mapSessionToken