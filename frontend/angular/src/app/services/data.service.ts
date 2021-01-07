import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {Usuario} from '../models/usuario';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class DataService {  
  private readonly API_URL = 'http://localhost:49999/api/usuario';    
  private readonly Token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MTAwNTE2NTcsImV4cCI6MTYxMDEzODA1NywiaWF0IjoxNjEwMDUxNjU3fQ.5m8b6EILAX-T2ckfn38iIoU-oxI1HGnTv7zVmRFGwYI'
  private readonly reqHeader = new HttpHeaders({ 
    'Authorization': 'Bearer ' + this.Token
  });  

  dataChange: BehaviorSubject<Usuario[]> = new BehaviorSubject<Usuario[]>([]);
  // Temporarily stores data from dialogs
  dialogData: any;

  constructor (private httpClient: HttpClient, private toastr: ToastrService) {}

  get data(): Usuario[] {
    return this.dataChange.value;
  }

  getDialogData() {
    return this.dialogData;
  }

  /** CRUD METHODS */
  listarUsuarios(): void {
    this.httpClient.get<Usuario[]>(this.API_URL + '/listar', { headers: this.reqHeader }).subscribe(data => {
        this.dataChange.next(data);
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          this.toastr.warning(error.error.errors);
        else
          this.toastr.warning(error.message);
      }
    );
  }

  adicionarUsuario(usuario: Usuario): void {
    this.httpClient.post(this.API_URL, usuario, { headers: this.reqHeader }).subscribe(data => {
        this.dialogData = usuario;
        this.toastr.success('Usuário adicionado com sucesso'); 

        setTimeout(() => {
          window.location.reload(); 
        }, 1500);        
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          this.toastr.warning(error.error.errors);
        else
          this.toastr.warning(error.message);                 
      }
    );
  }

  alterarUsuario(usuario: Usuario): void {
    this.httpClient.put(this.API_URL, usuario, { headers: this.reqHeader }).subscribe(data => {
        this.dialogData = usuario;
        this.toastr.success("Usuário alterado com sucesso");

        setTimeout(() => {
          window.location.reload(); 
        }, 1500); 
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          this.toastr.warning(error.error.errors);
        else
          this.toastr.warning(error.message);        
      }
    );
  }

  excluirUsuario(id: number): void {
    this.httpClient.delete(this.API_URL + '/' + id, { headers: this.reqHeader }).subscribe(data => {
      console.log(data['']);
        this.toastr.success("Usuário excluido com sucesso"); 

        setTimeout(() => {
          window.location.reload(); 
        }, 1500); 
      },
      (error: HttpErrorResponse) => {
        console.log (error.name + ' ' + error.message);
        
        if (error.error.errors)
          this.toastr.warning(error.error.errors);
        else
          this.toastr.warning(error.message);
      }
    );
  }

  listarEscolaridade() {    
    return this.httpClient.get(this.API_URL + '/escolaridades/listar', { headers: this.reqHeader });
  }
}




