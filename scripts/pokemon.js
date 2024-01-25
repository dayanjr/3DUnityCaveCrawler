const url = "https://pokeapi.co/api/v2/pokemon";

async function getPokemon(url){
    const resoponce = await fetch(url);
    if(resoponce.ok){
        const data = await resoponce.json();
        console.log(data);
        await renderPokemon(data);
        if(data.next != null){
            await getPokemon(data.next);
        }
    }
}

async function renderPokemon(data){
    const pokemon = data.results;
    const promises = pokemon.map(pokemonHtml);
    const html = await Promise.all(promises);
    const output = document.getElementById("pokemon");
    output.innerHTML += html.join("");
}
let s = ""
s.toUpperCase
async function pokemonHtml(data){
    const pokemonInfo = data.url;
    const pokeData = await getSinglePokemon(pokemonInfo);
    const img = pokeData.sprites.front_default;
    const name = pokeData.name.charAt(0).toUpperCase() + pokeData.name.slice(1);
    let types = pokeData.types;
    const html = `<div class="card">
        <h2>${name}</h2>
        <img src="${img}" alt="pokemon Image">
        <div class="card-content">
            <p> Type: ${types.map(()=>{
                if(types.length == 0){
                    return "";
                }
                else if (types.length == 1){
                    return types[0].type.name;
                }
                else{
                    const type = types[0].type.name + " " + types[1].type.name;
                    return type;
                }
            }).join("")}</p>
        </div>
    </div>`;
    return html;
}
async function getSinglePokemon(url){
    const resoponce = await fetch(url);
    if(resoponce.ok){
        const data = await resoponce.json();
        console.log(data.name);
        return data;
    }
}
getPokemon(url)