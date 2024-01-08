// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
import {levels} from "loglevel";

export const environment = {
  production: false,
  logLevel: levels.DEBUG,
  publicProjectName: "Sellify",
  apiHostUrl: "https://localhost:5001",
  fileSizeLimitBytes: 10485760,
  payments: {
    stripe: {
      pkey: "pk_test_51H05QBAPNszbnd282xR1ygdsHlQqVH6wzkWzWyNv4jlbLkQyhKjHl6UKBj1CbuURtwjvW1oeFXxSFo1Ixcue0lvv00HLlwwxMO"
    }
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
