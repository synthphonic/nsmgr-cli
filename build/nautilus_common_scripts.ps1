# ==========================================================================================================================
# nautilus_common_scripts.ps1
# ==========================================================================================================================
# Purpose:
#		A list of commonly used powershell functions for Nautilus powershell scripts
#
#
# Added By:
#		Shah Z. S
#
# First Seen:
#		18 Dec 2017, 14:07	 
#	
# Reference
# ==========================================================================================================================

Function global:IsConfigurationDefined($configurationName)
{
	$lowercase_config = $configurationName.ToLower();

	if ($lowercase_config.Equals("debug") -or ($lowercase_config.Equals("release")))
	{
		return $true;
	}

	Return $false;
}

Function global:GetOS() 
{
	if (![System.String]::IsNullOrWhiteSpace($env:OS))
	{
		if($env:OS.ToLower().Contains("windows"))
		{
			Return "Windows";
		}
	}

	Return "MacOS";
}

Function global:EmptyDirectory($targetPath)
{
	if([System.IO.Directory]::Exists($targetPath))
	{
		$dirInfo = [System.IO.DirectoryInfo]::new($targetPath)

		ForEach($fi in $dirInfo.GetFiles())
		{
			$fi.Delete();
		}

		ForEach($di in $dirInfo.GetDirectories())
		{
			EmptyDirectory($di.FullName);
			$di.Delete();
		}
	}
}

Function global:GetFiftyOneLabRootRepositoryPath()
{
	$hostOS = GetOS
	
	if ($hostOS.ToLower().Equals("windows"))
	{
		$rootPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSCommandPath,"..\..\..\..\"));
		Return $rootPath;
	}

	$rootPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSCommandPath,"../../../../"));
	Return $rootPath;
}

Function global:GetNautilusSolutionPath()
{
	$hostOS = GetOS
	
	if ($hostOS.ToLower().Equals("windows"))
	{
		$srcPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSCommandPath,"..\..\",$partialParentSrcFolder));
	}

	$srcPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSCommandPath,"../../",$partialParentSrcFolder));

	return $srcPath;
}

Function global:GetSolutionFile()
{
	$hostOS = GetOS

	if ($hostOS.ToLower().Equals("windows"))
	{				
		$global:solutionFileName = $windowsSolutionFileName
	}
	else 
	{
		$global:solutionFileName = $macSolutionFileName
	}
}

# Function global:UpFolder($path,$upPath)
# {	
# 	return [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($path, $upPath));	
# }