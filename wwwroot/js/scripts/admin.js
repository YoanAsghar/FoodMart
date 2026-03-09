//
//brings and shows all the products
//
const PRODUCTS_TABLE = document.getElementById("products-table");

async function getAllProductsFromDB(){
  try{
    const response = await fetch("http://localhost:5183/api/products")
    if(!response.ok) throw new Error("Error getting products");
    return await response.json();
  }catch(err){
    console.log(err);
    return null;
  }
}

async function renderProducts() {
    const products = await getAllProductsFromDB();
    const PRODUCTS_TABLE_BODY = document.querySelector("#products-table tbody");

    if (!products || !Array.isArray(products)) return;

    PRODUCTS_TABLE_BODY.innerHTML = "";

    for (let i = 0; i < products.length; i++) {
        const product = products[i];
        const row = `
            <tr id="product${product.id}">
                <td>${i + 1}</td>
                <td>${product.name}</td>
                <td>${product.category}</td>
                <td>$${product.price}</td>
                <td>
                    <button class="btn btn-sm btn-outline-secondary" id="edit-button-${product.id}">Edit</button>
                    <button class="btn btn-sm btn-outline-danger" id="delete-button-${product.id}">Delete</button>
                </td>
            </tr>
        `;
        PRODUCTS_TABLE_BODY.innerHTML += row;
    }
    
    // Once rendered, we activate the buttons
    AddDeleteFunctionToTheButtons(products);
}

async function AddDeleteFunctionToTheButtons(products){
  if (!products) return;
  
  for(let i = 0; i < products.length; i++){
    const product = products[i];
    const delete_button = document.getElementById(`delete-button-${product.id}`);

    if (delete_button) {
        delete_button.addEventListener("click", async () => {
          if(!confirm("Are you sure?")) return;
          
          try{
            const response = await fetch(`http://localhost:5183/api/products/${product.id}`,{
              method: "DELETE",
              headers: {
                'Authorization': `Bearer ${localStorage.getItem("jwt_token")}`
              }
            })
            if(!response.ok){
              console.log(response);
              return
            }
            const rowToRemove = document.getElementById(`product${product.id}`);
            if (rowToRemove) rowToRemove.remove();
          }catch(err){
            console.log(err)
          }
        })
    }
  }
}

//
//Create a new product
//

async function setupCreateProductForm(){
  const newProductForm = document.getElementById("product-form");
  const product_name = document.getElementById("product-name");
  const product_category = document.getElementById("product-category");
  const product_price = document.getElementById("product-price");
  const product_description = document.getElementById("product-description");
  const product_image = document.getElementById("product-image");

  if (!newProductForm) return;

  newProductForm.addEventListener("submit", async (e) => {
    e.preventDefault(); // Prevent page reload

    const newProduct = {
        name: product_name.value,
        description: product_description.value,
        price: parseFloat(product_price.value),
        imageUrl: product_image.value,
        category: product_category.value
    };

    try{
      const response = await fetch("http://localhost:5183/api/products", {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${localStorage.getItem("jwt_token")}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newProduct),
      });

      if(!response.ok){
        const errorData = await response.json();
        console.error("Error from server:", errorData);
        alert("Error creating product. Check console.");
        return;
      }

      alert("Product created successfully!");
      location.reload(); 

    }catch(err){
      console.error("Network or unexpected error:", err);
    }
  });
}

document.addEventListener("DOMContentLoaded", () => {
    renderProducts();
    setupCreateProductForm();
});
