import { Observable } from "rxjs";
import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

declare var toastr: any;

@Injectable()
export class ValidationErrorService {
    constructor() {
        this.reset();
    }

    message: string;
    icon: string;
    dismissable: boolean;
    header: string;
    imageIcon: string;
    iconColor: string;

    response: Response = null;
    
    reset() {
        this.message = "";
        this.header = "";
        this.dismissable = false;
        this.icon = "warning";
        this.imageIcon = "warning";
        this.iconColor = "inherit";
    }

    /**
     * Low level method to set message properties
     * @param msg - the message to set to
     * @param icon? - sets the icon property (warning*)
     * @param iconColor? - sets the icon color (left as is)
     */
    show(msg: string, icon?: string, iconColor?: string) {
        this.message = msg;
        this.icon = icon ? icon : "warning";
        if (iconColor)
            this.iconColor = iconColor;

        this.fixupIcons();


        if(this.icon == "warning")
          toastr.warning(this.message);
        if(this.icon == "info")
          toastr.info(this.message);
        if (this.icon == "success")
          toastr.success(this.message);
    }

    /**
     * Displays an error alert
     * @param msg  - Either a message string or error object with .message property
     */
    error(msg) {
        if (typeof (msg) === 'object' && msg.message)
            this.message = msg.message;
        else
            this.message = msg;

        this.show(this.message, "warning");
    }

    /**
     * DIsplays an info style alert
     * @param msg - message to display
     */
    info(msg) {
        this.show(msg, "info");
    }

    /**
     * Fixes up icons and colors based on standard icon settings
     * this method is called in internally after any of the helper
     * methods are called. You can call this when setting any icon
     * related properties manually.
     */
    fixupIcons() {
        var err = this;

        if (err.icon === "info")
            err.imageIcon = "info-circle";
        if (err.icon === "error" || err.icon === "danger" || err.icon === "warning") {
            err.imageIcon = "warning";
            err.iconColor = "firebrick";
        }
        if (err.icon === "success") {
            err.imageIcon = "check";
            err.iconColor = "green";
        }
    }

    /**
     * Parse a toPromise() .catch() clause error
     * from a response object and returns an errorInfo object
     * @param response
     * @returns {Promise<void>|Promise<T>}
     */
    parsePromiseResponseError(response) {

        if (response.hasOwnProperty("message"))
            return Promise.reject(response);
        if (response.hasOwnProperty("Message")) {
            response.message = response.Message;
            return Promise.reject(response);
        }

        let err = new ValidationErrorService();
        err.response = response;
        err.message = response.statusText;

        try {
            let data = response.json();
            if (data && data.message)
                err.message = data.message;
        }
        catch (ex) {

        }

        return Promise.reject(err);
    }

    parseObservableResponseError(response): Observable<any> {
        if (response.hasOwnProperty("message"))
            return Observable.throw(response);
        if (response.hasOwnProperty("Message")) {
            response.message = response.Message;
            return Observable.throw(response);
        }

        let err = new ValidationErrorService();
        err.response = response;
        err.message = response.statusText;

        try {
            let data = response.json();
            if (data && data.message)
                err.message = data.message;
        }
        catch (ex) { }

        if (!err.message)
            err.message = "Unknown server failure.";

        return Observable.throw(err);
    }

    
  focusInvalidControl(fg: FormGroup) {
    let invalid = <any[]>Object.keys(fg.controls).map(key => fg.controls[key]).filter(ctl => ctl.invalid);
    if (invalid.length > 0) {
      if('controls' in invalid[0]) {
        for(var i=0; i<invalid[0].controls.length; i++) {
          if(!invalid[0].controls[i].valid) {
            this.focusInvalidControl(invalid[0].controls[i] as FormGroup);
          }
        }
      } else {
        $((<any>invalid[0]).nativeElement).find('input,select').focus();
      }
    }
  }

  focusControl(fg: FormGroup) {
    let ctrls = <any[]>Object.keys(fg.controls).map(key => fg.controls[key]);
    if (ctrls.length > 0) {
      if('controls' in ctrls[0]) {
        this.focusInvalidControl(ctrls[0].controls[0] as FormGroup);
      } else {
        for(var i=0; i<ctrls.length; i++) {
          const element = (<any>ctrls[i]).nativeElement;
          if(element != null) {
            $(element).find('input,select').focus();
            return;
          }
        }
      }
    }
  }
}
