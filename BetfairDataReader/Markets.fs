namespace Betfair

open System
open System.Diagnostics.CodeAnalysis
open System.Net
open Betfair.Http
open FSharp.Data
open FSharp.Data.HttpRequestHeaders

module Markets =

    type EventType =
        | Soccer
        | Tennis
        | Golf
        | Cricket
        | RugbyUnion
        | Boxing
        | HorseRacing
        | MotorSport
        | SpecialBets
        | Cycling
        | RugbyLeague
        | Darts
        | Athletics
        | GreyhoundRacing
        | FinancialBets
        | Snooker
        | AmericanFootball
        | Baseball
        | Basketball
        | IceHockey
        | Chess
        | Poker
        | Handball
        | Volleyball
        | GaelicGames
        | Politics
        | MixedMartialArts

    type BspMarketsOption =
        | BspMarketsOnly
        | NonBspMarketsOnly
        | AllMarkets

    type BspMarketCategory =
        | BspMarket
        | NonBspMarket

    type InPlayMarketsOption =
        | InPlayEnabledMarketsOnly
        | InPlayDisabledMarketsOnly
        | AllMarkets

    type InPlayMarketCategory =
        | InPlayEnabledMarket
        | InPlayDisabledMarket

    type MarketBettingType =
        | Odds
        | AsianHandicapSingleLine
        | AsianHandicapDoubleLine

    type Country =
        | Argentina
        | Australia
        | Austria
        | Belarus
        | Belgium
        | Brazil
        | Cameroon
        | Canada
        | Chile
        | China
        | Colombia
        | Croatia
        | CzechRepublic
        | Denmark
        | Ecuador
        | Estonia
        | Finland
        | France
        | Germany
        | Hungary
        | Iceland
        | Ireland
        | Italy
        | Japan
        | Kazakhstan
        | Latvia
        | Lithuania
        | Netherlands
        | Morocco
        | NewZealand
        | Norway
        | Paraguay
        | Poland
        | Portugal
        | Russia
        | Singapore
        | Slovakia
        | Slovenia
        | SouthAfrica
        | SouthKorea
        | Spain
        | Sweden
        | Thailand
        | UnitedKingdom
        | UnitedStates
        | Uruguay

    type MarketType = 
        | MatchOdds
        | Place
        | Win

    type DateFrom = DateTimeOffset
    type DateTo = DateTimeOffset
    type DateRange = DateFrom * DateTo
    type MarketStartTime = DateTimeOffset
    type MarketSuspendedTime = DateTimeOffset
    type MarketSettledTime = DateTimeOffset

    type OrderStatus =
        | ExecutionComplete
        | Execuatable

    type TextQuery = string
    type EventId = string
    type CompetitionId = string
    type MarketId = string
    type MarketName = string
    type Venue = string

    type MarketFilter = {
        textQuery: TextQuery option;
        eventTypeIds: EventType list option;
        eventIds: EventId list option;
        competitionIds: CompetitionId list option;
        marketIds: MarketId list option;
        venues: Venue list option;
        bspOnly: BspMarketsOption;
        turnInPlayEnabled: InPlayMarketsOption;
        marketBettingTypes: MarketBettingType list option;
        marketCountries: Country list option;
        marketTypeCodes: MarketType list option;
        marketStartTime: DateRange option;
        withOrders: OrderStatus list option
    }

    type MarketSortOrder =
        | FirstToStart
        | LastToStart
        | VolumeLowToHigh
        | VolumeHighToLow
        | AvailableToMatchLowToHigh
        | AvailableToMatchHighToLow

    type MarketProjection =
        | Competition
        | Event
        | EventType
        | MarketStartTime
        | MarketDescription
        | RunnerDescription
        | RunnerMetadata

    type PersistenceCategory =
        | PersistenceEnabled
        | PersistenceDisabled

    type MarketRegulator = string
    type MarketBaseRate = double

    type DiscountCategory =
        | DiscountEnabled
        | DiscountDisabled

    type WalletType =
        | UnitedKingdomWallet
        | AustraliaWallet

    type MarketRules = string
    type MarketClarifications = string

    type RulesHasDateCategory =
        | RulesHasDate
        | RulesHasNoDate

    type EachWayDivisor = double

    type MarketDescription = {
        persistenceEnabled: PersistenceCategory;
        bspMarket: BspMarketCategory;
        marketTime: MarketStartTime;
        suspendTime: MarketSuspendedTime;
        settleTime: MarketSettledTime option;
        bettingType: MarketBettingType;
        turnInPlayEnabled: InPlayMarketCategory;
        marketType: MarketType;
        regulator: MarketRegulator;
        marketBaseRate: MarketBaseRate;
        discountAllowed: DiscountCategory;
        wallet: WalletType option;
        rules: MarketRules option;
        rulesHasDate: RulesHasDateCategory option;
        eachWayDivisor: EachWayDivisor option;
        clarifications: MarketClarifications
    }

    type TotalMatchedAmount = double

    type CompetitionName = string
    type Competition = CompetitionId * CompetitionName

    type EventName = string
    type EventStartTime = DateTimeOffset
    type Event = {
        id: EventId;
        name: EventName;
        country: Country;
        timezone: string;
        venue: Venue
        openDate: EventStartTime
    }

    type SelectionId = string
    type SelectionName = string
    type Handicap = double
    type SortOrder = int
    type SelectionMetadata = Map<string, string>

    type SelectionCatalogue = {
        selectionId: SelectionId;
        selectionName: SelectionName;
        handicap: Handicap
        sortPriority: SortOrder;
        metadata: SelectionMetadata
    }

    type MarketCatalogue = {
        marketId: MarketId;
        marketName: MarketName;
        marketStartTime: MarketStartTime option;
        description: MarketDescription option;
        totalMatched: TotalMatchedAmount option;
        runners: SelectionCatalogue list option;
        eventType: EventType option;
        competition: Competition option;
        event: Event option 
    }

    let createListMarketCataloguesRequest () =
        HttpRequest (
            "https://api.betfair.com/exchange/betting/rest/v1.0/listMarketCatalogue/",
            WebRequestMethods.Http.Post,
            [ "X-Application", "4ztzpQ311aLreFka"; ContentType HttpContentTypes.FormValues ],
            FormValues [ "username", "joelfk"; "password", "g!250953" ],
            None)

    let convertToLoginResponse loginResponseDeserializer httpResponse =
        match httpResponse with
        | HttpSuccess body -> loginResponseDeserializer body
        | HttpFailure _ ->  (OtherError, None) |> LoginResponse

    [<ExcludeFromCodeCoverage>]
    let listMarketCatalogues () =
        createListMarketCataloguesRequest ()
        |> executeHttpRequest
        |> convertToLoginResponse loginResponseDeserializer