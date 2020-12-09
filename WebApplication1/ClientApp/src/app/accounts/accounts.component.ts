import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizeService } from 'src/api-authorization/authorize.service';
import { FetcherService } from '../fetcher.service';
import { MongoAccount } from '../models/mongo-account';

@Component({
  selector: 'app-accounts',
  templateUrl: './accounts.component.html',
  styleUrls: ['./accounts.component.css']
})
export class AccountsComponent implements OnInit {
  public Accounts: Array<MongoAccount>;
  public User: MongoAccount;

  constructor(private fetcher: FetcherService, private authorizeService: AuthorizeService, private router: Router) { 
  }

  ngOnInit() {
    var i = 0;
    this.authorizeService.isAuthenticated().subscribe(auth => {
      if (i++ === 0) {
        if (!auth) {
          this.router.navigate(["/authentication/login"]);
        } else {
          this.getAccounts(sessionStorage.getItem('username'));
          this.fetcher.getAccountFromApi(sessionStorage.getItem('username')).subscribe(resp => this.User = resp, err => console.log(err));
        }
      }
    });
  }

  getAccounts(username: string) {
    this.fetcher.getAccountsFromApi().subscribe(resp => {
      if (!this.Accounts) {
        this.Accounts=resp;
        console.log(resp);
      }
    }, error => console.log(error));
  }

  edit(idx: number) {
    this.router.navigate(["accounts", this.Accounts[idx].id]);
  }
}
