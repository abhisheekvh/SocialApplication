import { Component, inject } from '@angular/core';
import{FormsModule} from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-nav',
  standalone:true,
  imports: [FormsModule,NgIf],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  model:any={};
  loggedin=false;
  name:any
  private accountservice=inject(AccountService);
  login()
  {
      this.accountservice.login(this.model).subscribe({
        next:response=>{
          this.name=response
          console.log(response);
        this.loggedin=true
        },
        error: error=>console.log(error)
        
      })
  }
  logout()
  {
    this.loggedin=false;
  }
}
