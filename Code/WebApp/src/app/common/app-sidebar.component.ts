import { Component, OnInit } from '@angular/core';
import {AppConfiguration} from '../business/appConfiguration';
import { UserInfo } from '../business/userInfo';
declare var $: any;

@Component({
    selector: 'app-sidebar',
    templateUrl: 'app-sidebar.component.html'
})
export class AppSidebarComponent implements OnInit {
    constructor(private config: AppConfiguration, private user: UserInfo) {
    }

    ngOnInit() {

        // Hide submenus
        $('#body-row .collapse').collapse('hide'); 

        // Collapse/Expand icon
        $('#collapse-icon').addClass('fa-angle-double-left'); 

        const SidebarCollapse = function (): void {
            $('.menu-collapsed').toggleClass('d-none');
            $('.sidebar-submenu').toggleClass('d-none');
            $('.submenu-icon').toggleClass('d-none');
            $('#sidebar-container').toggleClass('sidebar-expanded sidebar-collapsed');

            // Treating d-flex/d-none on separators with title
            const SeparatorTitle = $('.sidebar-separator-title');
            if ( SeparatorTitle.hasClass('d-flex') ) {
                SeparatorTitle.removeClass('d-flex');
            } else {
                SeparatorTitle.addClass('d-flex');
            }

            // Collapse/Expand icon
            $('#collapse-icon').toggleClass('fa-angle-double-left fa-angle-double-right');
        };

        // Collapse click
        $('[data-toggle=sidebar-colapse]').click(function() {
            SidebarCollapse();
        });
    }
}
