"..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -target:"..\..\..\packages\NUnit.ConsoleRunner.3.2.1\tools\nunit3-console.exe" -targetargs:"BetfairSimulator.Tests.dll" -filter:"+[BetfairSimulator]Betfair.*" -excludebyattribute:*.ExcludeFromCodeCoverage*^ -register:user -output:"_CodeCoverageResult.xml
"..\..\..\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe" "-reports:_CodeCoverageResult.xml" "-targetdir:_CodeCoverageReport"