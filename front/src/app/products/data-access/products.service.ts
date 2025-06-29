import { Injectable, inject, signal } from "@angular/core";
import { Product } from "./product.model";
import { HttpClient } from "@angular/common/http";
import { catchError, firstValueFrom, Observable, of, tap } from "rxjs";
import { environment } from "environments/environment";

@Injectable({
  providedIn: "root"
})
export class ProductsService {

  private readonly http = inject(HttpClient);
  private readonly path = environment.API

  private _products = signal<Product[]>([]);
  public readonly products = this._products.asReadonly();

  public get(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.path}GetAllProduct`).pipe(
      tap(products => this._products.set(products)),
      catchError(error => {
        console.error("Erreur lors de la récupération des produits :", error);
        return of([]); // retourne un tableau vide si erreur
      })
    );
  }

 async login(): Promise<any> {
    const dataUser = {
      Email: 'Admin@admin.com',
      Password: 'MotDePasseSecurise1234'
    };
 
    try {
      const result = await firstValueFrom(
        this.http.post<any>(`${this.path}token`, dataUser).pipe(
          tap(success => {
            if (success) {
              localStorage.setItem('user', JSON.stringify(success));
            }
          }),
          catchError(error => {
            console.error('Erreur lors de la création du produit:', error);
            return of(false);
          })
        )
      );
 
      return result;
    } catch (error) {
      console.error('Error en login:', error);
      return null;
    }
  }
  public create(product: Product): Observable<Product|null> {
    
   const token = JSON.parse(localStorage.getItem('user') || '{}')?.token;
   const headers = {
    Authorization: `Bearer ${token}`
  };

    return this.http.post<Product>(`${this.path}CreerUnProduit`, product, {headers}).pipe(
      tap(newProduct => {
        if (newProduct) {
          
          this._products.update(products => [newProduct, ...products]);
        }
      }),
      catchError(error => {
        console.error("Erreur lors de la création du produit :", error);
        return of(null);
      })
    );
  }


  public update(product: Product): Observable<boolean> {
    const token = JSON.parse(localStorage.getItem('user') || '{}')?.token;
   const headers = {
    Authorization: `Bearer ${token}`
  };
    return this.http.put<boolean>(`${this.path}UpdateProduct/${product.id}`, product,{headers}).pipe(
      tap(success => {
        if (success) {
          
          this._products.update(products =>
            products.map(p => p.id === product.id ? product : p)
          );
        }
      }),
      catchError(error => {
        console.error("Erreur lors de la mise à jour du produit :", error);
        return of(false);
      })
    );
  }

  public delete(productId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.path}DeleteProduct/${productId}`).pipe(
      tap(success => {
        if (success) {
          this._products.update(products =>
            products.filter(product => product.id !== productId)
          );
        }
      }),
      catchError(error => {
        console.error("Erreur lors de la suppression du produit :", error);
        return of(false);
      })
    );
  }
}
