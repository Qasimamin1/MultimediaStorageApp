import React, { useState, useEffect } from 'react';
import { Table } from 'react-bootstrap';

//functional component
function UploadForm() {
  // file for storing the selected file and media for storing media data fetched from the backend.
  const [file, setFile] = useState(null);
  const [media, setMedia] = useState(null);

// FUNCTION 1 updates the file state when a new file is selected in the file input.
  const handleFileChange = (event) => {
    
    setFile(event.target.files[0]);
  };

  useEffect(() => {
    fetchData();
  }, []);

  // function 2.. fetchData to retrieve media data from the backend.
  const fetchData = async () => {
    
    fetch('https://localhost:7143/get-media', { //GET request to the backend endpoint 
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    })
    //Handles the response from the fetch request. Checks if the request was successful (response.ok).
      .then(response => {
        if (!response.ok) {
          const errorData = response.json();
          alert(errorData.message);
        }
        return response.json();
      })
      //Updates the media state with the data received from the response.
      .then(data => {   
        setMedia(data.data);
      })
      .catch(error => {
        console.error('There was a problem with your fetch operation:', error);
      });
  };

    // function no 3 handleSubmit for handling the form submission to upload a file.
  const handleSubmit = async (event) => 
  {
    event.preventDefault();   //By calling event, you prevent this reload, allowing you to handle the submission with JavaScript.
    if (!file) {
      alert('Please select a file first!');
      return;
    }
      //Checks if a file is selected before proceeding with the upload.
    const formData = new FormData();
    formData.append('file', file);  

    try {
      //Prepares the file data for sending to the backend.

      const response = await fetch('https://localhost:7143/add-media', {
        method: 'POST',
        body: formData,  //body of the request is the formData containing the file.
      });
      //POST request to the backend endpoint 
      if (response.ok) {
        const responseData = await response.json();
        alert(responseData.message);
        fetchData();
        window.location.reload();
      } else {
        const errorData = await response.json();
        alert(errorData.message);
      }
    } catch (error) {
      alert('An error occurred', error);
    }
  };

 // function deleteItem that takes itemId as a parameter.
  const deleteItem = async (itemId) => {
    try {
      // Make the API call to delete the item
      const response = await fetch(`https://localhost:7143/delete-media?file=${itemId}`, {
        method: 'GET',  //Specifies the HTTP method and headers for the fetch request.
        headers: {
          'Content-Type': 'application/json',
        },
      });
 // check if response was successful
      if (response.ok) {

        const responseData = await response.json(); //converts the response data into a JavaScript object.
        alert(responseData.message);
        fetchData();  //re-fetches the data from the server 
        window.location.reload(); //refresh the current document.
      } else {
        const errorData = await response.json();
        alert(errorData.message);
      }
    } catch (error) {
      alert('An error occurred', error);
    }
  };


  return (
    <>
      <div className='container'>

        <form onSubmit={handleSubmit}>
          <div className='row mt-5 mb-5'>
            <div className='align-center'>
              <h1>File Upload</h1>
            </div>
          </div>
          <div className='row' style={{ marginBottom: '3%' }}>
            <div className='align-center '>
              <input className='btn btn-primary col-md-4' type="file" onChange={handleFileChange} />
              <button className='btn btn-primary' style={{ marginLeft: '20%' }} type="submit">Upload</button>
            </div>
          </div>
        </form>

        <div>
          <Table striped bordered hover>
            <thead>
              <tr>
                <th scope="col">#</th>
                <th scope="col">Name</th>
                <th scope="col">Is Delete</th>
                <th scope="col">Action</th>
              </tr>
            </thead>
            <tbody>
              {media ? (
                <>
                  {media.map((item, index) => (
                    <tr key={index}>
                      <th>{item.id}</th>
                      <th>{item.fileName}</th>
                      <td>{item.isDelete == 0 ? "False" : "True"}</td>
                      <td><button className='btn btn-danger' onClick={() => deleteItem(item.id)}>Delete</button></td>
                    </tr>
                  ))}
                </>
              ) : (
                'Loading...'
              )}
            </tbody>
          </Table>
        </div>
      </div>
    </>
  );

}

export default UploadForm;
