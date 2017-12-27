import {Component} from '@angular/core';
import { AppConfiguration } from "../business/appConfiguration";

@Component({
  selector: 'about',
  templateUrl: 'about.html'
})
export class AboutComponent {
    constructor(private config: AppConfiguration) {
        config.activeTab = "about";
    }
}
