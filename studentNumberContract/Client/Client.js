const { Web3 } = require('web3');
const { readFileSync } = require('fs');

// Since you are using Ganache, connect to the Ganache RPC server
const ganacheUrl = 'http://127.0.0.1:7545'; // This is the default, but check your Ganache settings
const web3 = new Web3(ganacheUrl);

// Replace with your contract's ABI
const contractABI = JSON.parse(readFileSync('C:\\Users\\david\\Documents\\GitHub\\LOG721_Lab01\\studentNumberContract\\build\\contracts\\StudentContract.json', 'utf-8')).abi;

// Replace with your contract's address from Ganache
const contractAddress = '0xA97c1cfB05925c3758Ce4b2fE83A73A198c9111C';

// The contract instance
const contract = new web3.eth.Contract(contractABI, contractAddress);

// Replace with one of the Ganache provided addresses that will interact with the contract
const fromAddress = '0xA97c1cfB05925c3758Ce4b2fE83A73A198c9111C'; // Truncated for brevity

// Function to set the student number in the contract
const setStudentNumber = async (studentNumber) => {
    const gasPrice = await web3.eth.getGasPrice();
    const gasEstimate = await contract.methods.setStudentNumber(studentNumber).estimateGas({ from: fromAddress });

    contract.methods.setStudentNumber(studentNumber).send({
        from: fromAddress,
        gasPrice: gasPrice,
        gas: gasEstimate,
        // This value should be the required payment for setStudentNumber method
        // Make sure to check this value in your smart contract
        value: web3.utils.toWei('0.0054', 'ether')
    }).then(receipt => {
        console.log('Transaction receipt:', receipt);
    }).catch(error => {
        console.error('Error sending transaction:', error);
    });
};

// Function to get the student number from the contract
const getStudentNumber = async () => {
    const number = await contract.methods.getStudentNumber().call();
    console.log('Student Number:', number);
};

// Example usage:
// Replace 123456 with the actual student number you want to set
setStudentNumber(123456).then(getStudentNumber);
// Then retrieve it

