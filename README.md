# Abysalto
Riješenje ulaznog zadatka Backend Akademije.
Zadatk je bio izrada middleware koji mora imat mogućnost dohvata proizvoda s vanjskog izvora(DummJSON).

# Korištene tehnologije:
- .NET 10 / ASP.NET Core Web API
- Swashbuckle


## Pokretanje projekta lokalno

# Preduvjeti
- .NET 8 SDK ([download](https://dotnet.microsoft.com/download))

# Koraci
1. Kloniraj repozitorij:
```bash
   git clone <repo-url>
   cd Abysalto.API
```

2. Pokreni aplikaciju:
```bash
   dotnet run
```
ili otvaranjem solutiona u Visual Studio i pokretanja aplikacije preko zelenog start buttona

3. Otvori Swagger dokumentaciju u browseru:
https://localhost:7163/swagger
(provjera porta u browseru ili terminalu,može se razlikovati)

Rješenje ima 7 implementiranih endpointa:
1. Endpoint koji vraća listu proizvoda
2. Endpoint koji vraća detalje jednog proizvoda
3. Endpoint koji omogućava filtriranje po kategoriji i cijeni
4. Endpoint koji za uneseni tekst pretražuje proizvode po nazivu
5. Endpoint koji zove login
6. Endpoint koji refresha token
7. Endpoint koji validira ispravnost tokena

## 1. Endpoint koji vraća listu proizvoda
**Ruta:** GET `/Product`
**Opis:** Lista svih proizvoda (slika, naziv, cijena, skraćeni opis do 100 znakova). Rezultat se cachira 5 minuta kako bi se izbjeglo ponovno pozivanje DummyJSON-a.

## 2. Endpoint koji vraća detalje jednog proizvoda
**Ruta:** GET `/Product/{id}`
**Opis:** Vraća pune detalje jednog proizvoda po ID-u. Ako proizvod ne postoji, vraća `404 Not Found`.

## 3. Endpoint koji omogućava filtriranje po kategoriji i cijeni
**Ruta:** GET `/Product/filter`
**Opis:** Filtrira proizvode po kategoriji i/ili rasponu cijene. Svi parametri su opcionalni i mogu se kombinirati. Kategorija se dohvaća server-side (DummyJSON endpoint), dok se cijena filtrira lokalno jer to DummyJSON ne podržava.

## 4. Endpoint koji za uneseni tekst pretražuje proizvode po nazivu
**Ruta:** GET `/Product/search/{search}`
**Opis:** Pretražuje proizvode po nazivu koristeći DummyJSON-ov server-side search endpoint. Parametar `term` je obavezan.

## 5. Endpoint koji zove login
**Ruta:** POST `/Auth/login`
**Opis:** Prima `username` i `password`, provjerava kredencijale putem DummyJSON-a, te vraća `accessToken` i `refreshToken`. Primjer tijela zahtjeva:
```json
{
  "username": "emilys",
  "password": "emilyspass"
}
```

## 6. Endpoint koji refresha token
**Ruta:** POST `/Auth/refresh`
**Opis:** Prima važeći `refreshToken`, vraća `accessToken` bez potrebe za ponovnim unosom kredencijala.

## 7. Endpoint koji validira ispravnost tokena
**Ruta:** GET `/Auth/validate`
**Opis:** Provjerava je li poslani `accessToken` i dalje važeći.

## Korištenje AI alata
Kod upotrebe AI alata sam koristio većinom Claude, kod pitanja oko arhitekture projekta i implementaciju HTTP poziva prema DummyJSON API-ju.