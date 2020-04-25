# VTS API
This API allows you to send the required paramters for the type of data you would like to return

## Endpoints

* `/api/v1/spectral`
* `/api/v1/forward`
* `/api/v1/inverse`

## Spectral Endpoint Parmeters

### __Spectral Properties__

|Property|Type|Values|Required|
|---|---|---|:---:|
|SpectralPlotType|string|musp|yes
|||mua|
|PlotName|string|`Any<string>`|yes
|TissueType|string|`Any<string>`|yes
|AbsorberConcentration*|array|`Array<Object>`|yes
|BloodConcentration*|array|`Object`|yes
|ScattererType|string|PowerLaw|yes
|||Intralipid|
|||Mie|
|PowerLawScatterer*|array|`Object`|yes if scatterer type is PowerLaw
|IntralipidScatterer*|array|`Object`|yes if scatterer type is Intralipid
|MieScatterer*|array|`Object`|yes if scatterer type is Mie
|XAxis*|array|`Object`|yes

_*Objects defined in detail below_

### __Absorber concentration object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|label|string|Hb|yes
|||HbO2|
|||H2O|
|||Fat|
|||Melanin|
|value|double|`Any<double>`|yes
|units|string|`Any<string>`|yes

### __Blood concentration object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|totalHb|double|`Any<double>`|yes
|bloodVolume|double|`Any<double>`|yes
|stO2|double|`Any<double>`|yes

### __Power Law scatterer object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|a|double|`Any<double>`|yes
|b|double|`Any<double>`|yes

### __Intralipid scatterer object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|VolumeFraction|double|`Any<double>`|yes

### __Mie Scatterer object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|ParticleRadius|double|`Any<double>`|yes
|ParticleRefractiveIndex|double|`Any<double>`|yes
|MediumRefractiveIndex|double|`Any<double>`|yes
|VolumeFraction|double|`Any<double>`|yes

### __X-Axis object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|Axis|string|`Any<string>`|yes
|AxisRange*|double|`Object`|yes

_*Object defined in detail below_

### __AxisRange object__

|Property|Type|Values|Required|
|---|---|---|:---:|
|Start|double|`Any<double>`|yes
|Stop|double|`Any<double>`|yes
|Count|double|`Any<double>`|yes

## Sample Payload
```json
{
    "spectralPlotType": "musp",
    "plotName": "μs'",
    "tissueType": "Skin",
    "absorberConcentration": [
        {
            "label": "Hb",
            "value": 28.4,
            "units": "μM"
        },
        {
            "label": "HbO2",
            "value": 22.4,
            "units": "μM"
        },
        {
            "label": "H2O",
            "value": 0.7,
            "units": "vol. frac."
        },
        {
            "label": "Fat",
            "value": 0,
            "units": "vol. frac."
        },
        {
            "label": "Melanin",
            "value": 0.0051,
            "units": "vol. frac."
        }
    ],
    "bloodConcentration": {
        "totalHb": 50.8,
        "bloodVolume": 0.021844,
        "stO2": 0.4409448818897638
    },
    "scatteringType": "PowerLaw",
    "powerLawScatterer": {
        "a": 1.2,
        "b": 1.42
    },
    "intralipidScatterer": {
        "volumeFraction": 0.01
    },
    "mieScatterer": {
        "particleRadius": 0.5,
        "particleRefractiveIndex": 1.4,
        "mediumRefractiveIndex": 1,
        "volumeFraction": 0.01
    },
    "xAxis": {
    	"axis": "wavelength",
    	"axisRange": {
        	"start": 650,
        	"stop": 1000,
        	"count": 8
    	}
    }
}
```
## Sample Result

```json
{
    "id": "SpectralMusp",
    "plotList": [
        {
            "label": "Skin μs'",
            "data": [
                [
                    650.0,
                    2.2123013258570863
                ],
                [
                    700.0,
                    1.9913244640761809
                ],
                [
                    750.0,
                    1.8054865273011322
                ],
                [
                    800.0,
                    1.6473787686682118
                ],
                [
                    850.0,
                    1.5114938050444233
                ],
                [
                    900.0,
                    1.3936601517386393
                ],
                [
                    950.0,
                    1.290665574863872
                ],
                [
                    1000.0,
                    1.2
                ]
            ]
        }
    ]
}
```

## Forward Solver Parameters

```json
{
    "forwardSolverType": "PointSourceSDA",
    "solutionDomain": "ROfFxAndTime",
    "xAxis": {
    	"axis": "time",
    	"axisRange": {
        	"start": 0.001,
        	"stop": 0.05,
        	"count": 51
    	}
    },
    "independentAxis": {
    	"axis": "fx",
    	"axisRange": {
        	"start": 0,
        	"stop": 0.5,
        	"count": 11
    	}
    },
    "opticalProperties": {
	        "mua": 0.06,
	        "musp": 1.06,
	        "g": 0.8,
	        "n": 1.4
    },
    "modelAnalysis": "R",
    "noiseValue": "0"
}
```

## Inverse Solver Parameters

```json
{
    "forwardSolverEngine": "PointSourceSDA",
    "optimizerType": "MPFitLevenbergMarquardt",
    "solutionDomain": "rofrho",
    "xAxis": {
    	"axis": "rho",
    	"axisRange": {
        	"start": 0.5,
        	"stop": 9.5,
        	"count": 9
    	}
    },
    "independentAxis": {
    	"axis": "time",
    	"axisValue": 0.05
    },
    "inverseSolverEngine": "PointSourceSDA",
    "opticalProperties": {
        "mua": 0.01,
        "musp": 1,
        "g": 0.8,
        "n": 1.4
    },
    "measuredData":[[0.5,0.0352304381546136],[1.0,0.014368953131916435],[1.5,0.0077256224944451043],[2.0,0.0046414734076406089],[2.5,0.0029591061550317734],[3.0,0.0019567204760751406],[3.5,0.001326067149533223],[4.0,0.00091467057173402289],[4.5,0.000639390319322507],[5.0,0.00045170719547787871],[5.5,0.00032189268619509397],[6.0,0.00023106891488301496],[6.5,0.00016692267886029029],[7.0,0.00012125558671965844],[7.5,8.8519676438976783E-05],[8.0,6.4910942032566415E-05],[8.5,4.779256318127966E-05],[9.0,3.5319800076064575E-05],[9.5,2.6191692417237693E-05]]
}
```

